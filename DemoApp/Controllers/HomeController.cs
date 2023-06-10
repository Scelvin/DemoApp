using DemoApp.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;

namespace DemoApp.Controllers
{
    public class HomeController : Controller
    {
        demoEntities db = new demoEntities();
        public ActionResult Index()
        {
            return View(db.users.ToList());
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(user users)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Notification = "Error";
                return View();
            }

            if (db.users.Any(x => x.username == users.username))
            {
                ViewBag.Notification = "This account already exists.";
                return View();
            }
            else
            {
                //users.password = HashPassword(users.password);
                db.users.Add(users);
                db.SaveChanges();

                Session["idSS"]       = users.id.ToString();
                Session["usernameSS"] = users.username.ToString();

                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(user users)
        {
            var checkLogin = db.users.Where(x => x.username.Equals(users.username) && x.password.Equals(users.password)).FirstOrDefault();
            if (checkLogin != null)
            {
                Session["idSS"] = users.id.ToString();
                Session["usernameSS"] = users.username.ToString();

                return RedirectToAction("Index", "Home");
            } else
            {
                ViewBag.Notification = "Wrong Username or Password";
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            using (demoEntities dbmodel = new demoEntities())
            {
                var users = dbmodel.users.Where(x => x.id == id).FirstOrDefault();
                return View(users);
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, user user)
        {
            try
            {
                user.confirmpassword = user.password;
                using (demoEntities dbmodel = new demoEntities())
                {
                    dbmodel.Entry(user).State = EntityState.Modified;
                    dbmodel.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            using (demoEntities dbmodel = new demoEntities())
            {
                return View(dbmodel.users.Where(x => x.id == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (demoEntities dbmodel = new demoEntities())
                {
                    user user = dbmodel.users.Where(x => x.id == id).FirstOrDefault();
                    dbmodel.users.Remove(user);
                    dbmodel.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] hash;

            using (var bcrypt = new Rfc2898DeriveBytes(password, 16, 10000))
            {
                salt = bcrypt.Salt;
                hash = bcrypt.GetBytes(20); // 20 is the length of the hash
            }

            // Combine the salt and hash into a single string for storage
            var saltBase64 = Convert.ToBase64String(salt);
            var hashBase64 = Convert.ToBase64String(hash);
            var hashedPassword = $"{saltBase64}:{hashBase64}";

            return hashedPassword;
        }

    }

}
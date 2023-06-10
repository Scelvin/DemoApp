using DemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoApp.Controllers
{
    public class HomeController : Controller
    {
        demoEntities db = new demoEntities();
        // GET: Home
        public ActionResult Index()
        {
            this.Register(); //
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Register(user users)
        {
            if (db.users.Any(x=>x.username == users.username)) {
                ViewBag.Notification = "This account already exists.";
                return View();
            } else {
                db.users.Add(users);
                db.SaveChanges();

                Session["idSS"]       = users.id.ToString();
                Session["usernameSS"] = users.username.ToString();

                return RedirectToAction("Index", "Home");
            }
        }
    }
}
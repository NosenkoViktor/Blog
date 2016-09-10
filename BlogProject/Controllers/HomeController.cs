using BlogProject.Abstract;
using BlogProject.Concrete;
using BlogProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogProject.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult RecentPosts()
        {
            return View();
        }

        public ActionResult Arhive()
        {
            return View();
        }

        public ActionResult Information(int id)
        {
            EFUserRepository model = new EFUserRepository();
            UserModel person = new UserModel();
            foreach (var p in model.Users)
            {
                if (id == p.ID) person = p;
            }
            return View(person);
        }
    }
}

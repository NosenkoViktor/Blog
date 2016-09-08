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
        private IUserRepository repository;

        public HomeController(IUserRepository usersRepository)
        {
            repository = usersRepository;
        }

        public ActionResult RecentPosts()
        {
            return View();
        }

        public ActionResult Arhive()
        {
            return View();
        }

        public ActionResult Information()
        {
            return View();
        }


        public ActionResult Test()
        {
            EFUserRepository model = new EFUserRepository();
            return View(model);
        }
    }
}

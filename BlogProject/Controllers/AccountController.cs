using BlogProject.Concrete;
using BlogProject.Models;
using BlogProject.Models.AccountModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BlogProject.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterUserModel model)
        {
            UserModel user = new UserModel();
            EFDbContext context = new EFDbContext();
            int count = context.Users.Count(u => u.Username == model.Username);
            if (ModelState.IsValid && count == 0)
            {
                user.Username = model.Username;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Password = model.Password;
                context.Users.Add(user);
                context.SaveChanges();
                return RedirectToAction("logIn", "Account");
            }
            else
            {
                ModelState.AddModelError("", "This Username is already in use. Please choose another Username.");
                return View();
            }
        }

        public JsonResult CheckUserName(string username)
        {
            var result = Membership.FindUsersByName(username).Count == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

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
        private UserModel user;
        private EFDbContext dbRepo;

        public AccountController(UserModel user, EFDbContext dbRepository)
        {
            this.user = user;
            dbRepo = dbRepository;
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("News", "Home");
        }

        [HttpPost]
        public ActionResult Register(RegisterUserModel model)
        {
            int count = dbRepo.Users.Count(u => u.Username == model.Username);
            if (ModelState.IsValid && count == 0)
            {
                user.Username = model.Username;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Password = model.Password;
                user.Roles = "user";
                dbRepo.Users.Add(user);
                dbRepo.SaveChanges();
                FormsAuthentication.SetAuthCookie(model.Username, true);
                string actionString = "Information/" + model.Username;
                return RedirectToAction(actionString, "Home", model.Username);
            }
            else
            {
                ModelState.AddModelError("", "This Username is already in use. Please choose another Username.");
                return View();
            }
        }

        [HttpPost]
        public ActionResult LogIn(LogInModel model)
        {
            int count = dbRepo.Users.Count(u => u.Username == model.Username);
            if (ModelState.IsValid && count == 1)
            {
                UserModel currentUser = dbRepo.Users.First(u => u.Username == model.Username);
                if (currentUser.Password == model.Password)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                    string actionString = "Information/" + currentUser.Username;
                    return RedirectToAction(actionString, "Home", currentUser.Username);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Password");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "We did not find you in the database. Check the correct input Username and Password or register");
                return View();
            }
        }
    }
}

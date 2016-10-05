using BlogProject.Concrete;
using BlogProject.Entity;
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
        private UserModel userModel;
        private EFUserRepository userRepo;

        public AccountController(UserModel user, EFUserRepository userRepository)
        {
            userModel = user;
            userRepo = userRepository;
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
        public ActionResult Register(RegisterUserModel info)
        {
            Users user = new Users();
            int count = userRepo.Users.Count(u => u.Username == info.Username);
            if (ModelState.IsValid && count == 0)
            {
                user.Username = info.Username;
                user.FirstName = info.FirstName;
                user.LastName = info.LastName;
                user.Email = info.Email;
                user.Password = info.Password;
                user.Roles = "user";
                userRepo.Add(user);
                FormsAuthentication.SetAuthCookie(info.Username, true);
                string actionString = "Information/" + info.Username;
                return RedirectToAction(actionString, "Home", info.Username);
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
            int count = userRepo.Users.Count(u => u.Username == model.Username);
            if (ModelState.IsValid && count == 1)
            {
                Users currentUser = new Users();
                currentUser = userRepo.Users.First(u => u.Username == model.Username);
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

using BlogProject.Concrete;
using BlogProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogProject.Controllers
{
    public class AdminController : Controller
    {
        private EFDbContext dbRepo;
        private EFUserRepository userRepo;

        public AdminController(EFDbContext dbRepository, EFUserRepository userRepository)
        {
            dbRepo = dbRepository;
            userRepo = userRepository;
        }

        public ActionResult Index()
        {
            ViewBag.Roles = new SelectList(
                new List<SelectListItem>
                { 
                    new SelectListItem {Text = "administrator", Value = "administrator"},
                    new SelectListItem {Text = "moderator", Value = "moderator"},
                    new SelectListItem {Text = "user", Value = "user"}
                }, "Value", "Text");
            AdminViewModel model = new AdminViewModel();
            model.Users = userRepo.Users;
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int userId)
        {
            Users user = dbRepo.Users.FirstOrDefault(u => u.ID == userId);
            if (user.Roles != "administrator")
            {
                if (dbRepo.Comments.Count(u => u.UserId == userId) == 0 && dbRepo.Posts.Count(u => u.UserId == userId) == 0)
                {
                    dbRepo.Users.Remove(user);
                    dbRepo.SaveChanges();
                    TempData["message"] = "User whith the username: <<" + user.Username.ToString() + ">> has been successfully removed.";
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    List<Comments> commentBase = dbRepo.Comments.Where(u => u.UserId == userId).ToList();
                    foreach (var c in commentBase)
                    {
                        dbRepo.Comments.Remove(c);
                        dbRepo.SaveChanges();
                    }
                    List<Posts> postBase = dbRepo.Posts.Where(u => u.UserId == userId).ToList();
                    foreach (var p in postBase)
                    {
                        List<Comments> commentForPost = dbRepo.Comments.Where(u => u.PostId == p.PostId).ToList();
                        foreach (var comment in commentForPost)
                        {
                            dbRepo.Comments.Remove(comment);
                            dbRepo.SaveChanges();
                        }
                        dbRepo.Posts.Remove(p);
                        dbRepo.SaveChanges();
                    }
                    dbRepo.Users.Remove(user);
                    dbRepo.SaveChanges();
                    TempData["message"] = "User whith the username: <<" + user.Username.ToString() + ">> has been successfully removed.";
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                TempData["message"] = "You can not delete user with the username: <<" + user.Username.ToString() + ">> it is an administrator.";
                return RedirectToAction("Index", "Admin");
            }
        }

        [HttpPost]
        public ActionResult Save(int userId, string roles)
        {
            Users user = dbRepo.Users.FirstOrDefault(u => u.ID == userId);
            user.Roles = roles;
            dbRepo.Users.Attach(user);
            dbRepo.Entry(user).State = EntityState.Modified;
            dbRepo.SaveChanges();
            TempData["message"] = "User whith the username: <<" + user.Username.ToString() + ">> has been successfully saved.";
            return RedirectToAction("Index", "Admin");
        }
    }
}

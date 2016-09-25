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
        EFDbContext context = new EFDbContext();
        EFUserRepository usersRepository = new EFUserRepository();

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
            model.Users = usersRepository.Users;
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int userId)
        {
            UserModel user = context.Users.FirstOrDefault(u => u.ID == userId);
            if (user.Roles != "administrator")
            {
                if (context.Comments.Count(u => u.UserId == userId) == 0 && context.Posts.Count(u => u.UserId == userId) == 0)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                    TempData["message"] = "User whith the username: <<" + user.Username.ToString() + ">> has been successfully removed.";
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    List<CommentModel> commentBase = context.Comments.Where(u => u.UserId == userId).ToList();
                    foreach (var c in commentBase)
                    {
                        context.Comments.Remove(c);
                        context.SaveChanges();
                    }
                    List<PostModel> postBase = context.Posts.Where(u => u.UserId == userId).ToList();
                    foreach (var p in postBase)
                    {
                        List<CommentModel> commentForPost = context.Comments.Where(u => u.PostId == p.PostId).ToList();
                        foreach (var comment in commentForPost)
                        {
                            context.Comments.Remove(comment);
                            context.SaveChanges();
                        }
                        context.Posts.Remove(p);
                        context.SaveChanges();
                    }
                    context.Users.Remove(user);
                    context.SaveChanges();
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
            UserModel user = context.Users.FirstOrDefault(u => u.ID == userId);
            user.Roles = roles;
            context.Users.Attach(user);
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
            TempData["message"] = "User whith the username: <<" + user.Username.ToString() + ">> has been successfully saved.";
            return RedirectToAction("Index", "Admin");
        }
    }
}

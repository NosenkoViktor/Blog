using BlogProject.Concrete;
using BlogProject.Entity;
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
        private EFUserRepository userRepo;
        private EFPostRepository postRepo;
        private EFCommentRepository commentRepo;
        private AdminModel adminModel;

        public AdminController(EFUserRepository userRepository, AdminModel adminInfo, EFPostRepository postRepository, EFCommentRepository commentRepository)
        {
            userRepo = userRepository;
            postRepo = postRepository;
            commentRepo = commentRepository;
            adminModel = adminInfo;
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
            Users user = userRepo.Users.FirstOrDefault(u => u.ID == userId);
            if (adminModel.User.Roles != "administrator")
            {
                if (commentRepo.Comments.Count(u => u.UserId == userId) == 0 && postRepo.Posts.Count(u => u.UserId == userId) == 0)
                {
                    userRepo.Delete(user);
                    TempData["message"] = "User whith the username: <<" + adminModel.User.Username.ToString() + ">> has been successfully removed.";
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    adminModel.Comments = commentRepo.Comments.Where(u => u.UserId == userId);
                    foreach (var c in adminModel.Comments)
                    {
                        commentRepo.Delete(c);
                    }
                    adminModel.Posts = postRepo.Posts.Where(u => u.UserId == userId).ToList();
                    foreach (var p in adminModel.Posts)
                    {
                        adminModel.Comments = commentRepo.Comments.Where(u => u.PostId == p.PostId);
                        foreach (var comment in adminModel.Comments)
                        {
                            commentRepo.Delete(comment);
                        }
                        postRepo.Delete(p);
                    }
                    userRepo.Delete(user);
                    TempData["message"] = "User whith the username: <<" + adminModel.User.Username.ToString() + ">> has been successfully removed.";
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                TempData["message"] = "You can not delete user with the username: <<" + adminModel.User.Username.ToString() + ">> it is an administrator.";
                return RedirectToAction("Index", "Admin");
            }
        }

        [HttpPost]
        public ActionResult Save(int userId, string roles)
        {
            Users user = userRepo.Users.FirstOrDefault(u => u.ID == userId);
            user.Roles = roles;
            userRepo.Edit(user);
            TempData["message"] = "User whith the username: <<" + user.Username.ToString() + ">> has been successfully saved.";
            return RedirectToAction("Index", "Admin");
        }
    }
}

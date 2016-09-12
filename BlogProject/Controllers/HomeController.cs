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
        EFUserRepository model = new EFUserRepository();
        EFPostRepository modelPost = new EFPostRepository();

        public ActionResult RecentPosts(int id)
        {
            ViewBag.Person = model.Users.FirstOrDefault(u => u.ID == id);
            PostsViewModel viewModel = new PostsViewModel()
            {
                PostView = modelPost.Posts.Where(p => p.UserId == id).OrderByDescending(p => p.PostTime)
            };
            return View(viewModel);
        }

        public FileContentResult GetImage(int id)
        {
            UserModel person = model.Users.FirstOrDefault(u => u.ID == id);
            if (person != null)
                return File(person.ImageData, person.ImageMimeType);
            else return null;
        }

        public ViewResult Arhive(int id)
        {
            ViewBag.Person = model.Users.FirstOrDefault(u => u.ID == id);
            PostsViewModel viewModel = new PostsViewModel()
            {
                PostView = modelPost.Posts.Where(p => p.UserId == id).OrderByDescending(p => p.PostTime)
            };
            return View(viewModel);
        }

        public ActionResult Information(int id)
        {
            UserModel user = new UserModel();
            ViewBag.Person = model.Users.FirstOrDefault(u => u.ID == id);
            foreach (var p in model.Users)
            {
                if (id == p.ID) user = p;
            }
            return View(user);
        }
    }
}

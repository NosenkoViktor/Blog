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

        public ActionResult RecentPosts(string username)
        {
            int id = GetUserId(username);
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

        public ViewResult Arhive(string username)
        {
            int id = GetUserId(username);
            ViewBag.Person = model.Users.FirstOrDefault(u => u.ID == id);
            PostsViewModel viewModel = new PostsViewModel()
            {
                PostView = modelPost.Posts.Where(p => p.UserId == id).OrderByDescending(p => p.PostTime)
            };
            return View(viewModel);
        }

        public ActionResult Information(string username)
        {
            int id = GetUserId(username);
            UserModel user = new UserModel();
            ViewBag.Person = model.Users.FirstOrDefault(u => u.ID == id);
            foreach (var p in model.Users)
            {
                if (id == p.ID) user = p;
            }
            return View(user);
        }

        public int GetUserId(string username)
        {
            UserModel currentUser = model.Users.First(u => u.Username == username);
            int id = currentUser.ID;
            return id;
        }

        public ActionResult News()
        {
            NewsViewModel news = new NewsViewModel();
            news.PostView = modelPost.Posts;
            news.UserView = model.Users;
            return View(news);
        }
    }
}

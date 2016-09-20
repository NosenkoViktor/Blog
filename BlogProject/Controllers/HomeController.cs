using BlogProject.Abstract;
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
    public class HomeController : Controller
    {
        EFUserRepository model = new EFUserRepository();
        EFPostRepository modelPost = new EFPostRepository();
        EFCommentRepository modelComment = new EFCommentRepository();
        EFDbContext context = new EFDbContext();

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

        [Authorize]
        public ActionResult ChangeInformation(string username)
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

        [Authorize]
        [HttpPost]
        public ActionResult ChangeInformation(UserModel user, HttpPostedFileBase image)
        {
            ViewBag.Person = model.Users.First(u => u.Username == User.Identity.Name);
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    user.ImageMimeType = image.ContentType;
                    user.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(user.ImageData, 0, image.ContentLength); 
                }
                else
                {
                    user.ImageData = model.Users.First(u => u.Username == User.Identity.Name).ImageData;
                    user.ImageMimeType = model.Users.First(u => u.Username == User.Identity.Name).ImageMimeType;
                }
                context.Users.Attach(user);
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Information", "Home", new { username = User.Identity.Name });
            }
            else
            {
                return View();
            }
        }

        public int GetUserId(string username)
        {
            UserModel currentUser = model.Users.First(u => u.Username == username);
            int id = currentUser.ID;
            return id;
        }

        public ActionResult News()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Person = model.Users.First(u => u.Username == User.Identity.Name);
            }
            NewsViewModel news = new NewsViewModel();
            news.PostView = modelPost.Posts;
            news.UserView = model.Users;
            return View(news);
        }

        public ActionResult CurrentPost(int postId)
        {
            int userId = modelPost.Posts.First(p => p.PostId == postId).UserId;
            ViewBag.Person = model.Users.FirstOrDefault(u => u.ID == userId);
            CurrentPostModel currentPost = new CurrentPostModel();
            currentPost.Post = modelPost.Posts.First(p => p.PostId == postId);
            currentPost.Users = model.Users;
            currentPost.Comments = modelComment.Comments.Where(c => c.PostId == postId);
            return View(currentPost);
        }


        [HttpPost]
        public ActionResult AddComment(string postId, CurrentPostModel newComment)
        {
            int IdForPost;
            int.TryParse(postId, out IdForPost);
            if (User.Identity.IsAuthenticated && newComment.UserComment.CommentText != null)
            {
                CommentModel comment = new CommentModel();
                comment.CommentText = newComment.UserComment.CommentText;
                comment.CommentTime = DateTime.Now;
                comment.PostId = IdForPost;
                comment.UserId = model.Users.First(u => u.Username == User.Identity.Name).ID;
                context.Comments.Add(comment);
                context.SaveChanges();
                return RedirectToAction("CurrentPost", "Home", new { postId = IdForPost });
            }
            else 
            {
                return RedirectToAction("LogIn", "Account");
            }
        }
    }
}

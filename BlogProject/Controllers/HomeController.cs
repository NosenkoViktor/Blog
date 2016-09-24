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
        public ActionResult AddComment(string post, CurrentPostModel newComment)
        {
            int IdForPost;
            int.TryParse(post, out IdForPost);
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

        [HttpPost]
        public ActionResult DeleteComment(int commentId, int post)
        {
            CommentModel comment = context.Comments.FirstOrDefault(c => c.CommentId == commentId);
            context.Comments.Remove(comment);
            context.SaveChanges();
            return RedirectToAction("CurrentPost", "Home", new { postId = post }); ;
        }

        public ActionResult AddPost(string username)
        {
            ViewBag.Person = model.Users.First(u => u.Username == User.Identity.Name);
            return View();
        }

        [HttpPost]
        public ActionResult AddPost(PostModel newPost)
        {
            if (newPost.Title != null && newPost.PostText != null)
            {
                PostModel post = new PostModel();
                post.UserId = GetUserId(User.Identity.Name.ToString());
                post.Title = newPost.Title;
                post.PostText = newPost.PostText;
                post.PostTime = DateTime.Now;
                context.Posts.Add(post);
                context.SaveChanges();
                return RedirectToAction("RecentPosts", "Home", new { username = User.Identity.Name.ToString() });
            }
            else
            {
                ViewBag.Person = model.Users.First(u => u.Username == User.Identity.Name);
                return View();
            }
        }

        [HttpPost]
        public ActionResult DeletePost(int postId)
        {
            PostModel post = context.Posts.FirstOrDefault(p => p.PostId == postId);
            if (context.Comments.Count(c => c.PostId == postId) == 0)
            {
                context.Posts.Remove(post);
                context.SaveChanges();
                return RedirectToAction("Arhive", "Home", new { username = User.Identity.Name });
            }
            else
            {
                List<CommentModel> commentBase = context.Comments.Where(c => c.PostId == postId).ToList();
                foreach (var c in commentBase)
                {
                    context.Comments.Remove(c);
                }
                context.SaveChanges();
                context.Posts.Remove(post);
                context.SaveChanges();
                return RedirectToAction("Arhive", "Home", new { username = User.Identity.Name });
            }
        }
    }
}

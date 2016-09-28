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
        private EFUserRepository userRepo;
        private EFPostRepository postRepo;
        private EFCommentRepository commentRepo;
        private EFDbContext dbRepo;

        public HomeController(EFUserRepository userRepository, 
            EFPostRepository postRepository, 
            EFCommentRepository commentRepository, 
            EFDbContext dbRepository)
        {
            userRepo = userRepository;
            postRepo = postRepository;
            commentRepo = commentRepository;
            dbRepo = dbRepository;
        }

        public ActionResult RecentPosts(string username)
        {
            int id = GetUserId(username);
            ViewBag.Person = userRepo.Users.FirstOrDefault(u => u.ID == id);
            PostsViewModel viewModel = new PostsViewModel()
            {
                PostView = postRepo.Posts.Where(p => p.UserId == id).OrderByDescending(p => p.PostTime)
            };
            return View(viewModel);
        }

        public FileContentResult GetImage(int id)
        {
            UserModel person = userRepo.Users.FirstOrDefault(u => u.ID == id);
            if (person != null)
                return File(person.ImageData, person.ImageMimeType);
            else return null;
        }

        public ViewResult Arhive(string username)
        {
            int id = GetUserId(username);
            ViewBag.Person = userRepo.Users.FirstOrDefault(u => u.ID == id);
            PostsViewModel viewModel = new PostsViewModel()
            {
                PostView = postRepo.Posts.Where(p => p.UserId == id).OrderByDescending(p => p.PostTime),
                Users = userRepo.Users
            };
            return View(viewModel);
        }

        public ActionResult Information(string username)
        {
            int id = GetUserId(username);
            UserModel user = new UserModel();
            ViewBag.Person = userRepo.Users.FirstOrDefault(u => u.ID == id);
            foreach (var p in userRepo.Users)
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
            ViewBag.Person = userRepo.Users.FirstOrDefault(u => u.ID == id);
            foreach (var p in userRepo.Users)
            {
                if (id == p.ID) user = p;
            }
            return View(user);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangeInformation(UserModel user, HttpPostedFileBase image)
        {
            ViewBag.Person = userRepo.Users.First(u => u.Username == User.Identity.Name);
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
                    user.ImageData = userRepo.Users.First(u => u.Username == User.Identity.Name).ImageData;
                    user.ImageMimeType = userRepo.Users.First(u => u.Username == User.Identity.Name).ImageMimeType;
                }
                dbRepo.Users.Attach(user);
                dbRepo.Entry(user).State = EntityState.Modified;
                dbRepo.SaveChanges();
                return RedirectToAction("Information", "Home", new { username = User.Identity.Name });
            }
            else
            {
                return View();
            }
        }

        public int GetUserId(string username)
        {
            UserModel currentUser = userRepo.Users.First(u => u.Username == username);
            int id = currentUser.ID;
            return id;
        }

        public ActionResult News()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Person = userRepo.Users.First(u => u.Username == User.Identity.Name);
            }
            NewsViewModel news = new NewsViewModel();
            news.PostView = postRepo.Posts;
            news.UserView = userRepo.Users;
            return View(news);
        }

        public ActionResult CurrentPost(int postId)
        {
            int userId = postRepo.Posts.First(p => p.PostId == postId).UserId;
            ViewBag.Person = userRepo.Users.FirstOrDefault(u => u.ID == userId);
            CurrentPostModel currentPost = new CurrentPostModel();
            currentPost.Post = postRepo.Posts.First(p => p.PostId == postId);
            currentPost.Users = userRepo.Users;
            currentPost.Comments = commentRepo.Comments.Where(c => c.PostId == postId);
            return View(currentPost);
        }


        [HttpPost]
        public ActionResult AddComment(int post, CurrentPostModel newComment)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("LogIn", "Account"); 
            if (string.IsNullOrEmpty(newComment.UserComment.CommentText))
                return RedirectToAction("CurrentPost", "Home", new { postId = post });
            CommentModel comment = new CommentModel();
            comment.CommentText = newComment.UserComment.CommentText;
            comment.CommentTime = DateTime.Now;
            comment.PostId = post;
            comment.UserId = userRepo.Users.First(u => u.Username == User.Identity.Name).ID;
            dbRepo.Comments.Add(comment);
            dbRepo.SaveChanges();
            return RedirectToAction("CurrentPost", "Home", new { postId = post });
        }

        [HttpPost]
        public ActionResult DeleteComment(int commentId, int post)
        {
            CommentModel comment = dbRepo.Comments.FirstOrDefault(c => c.CommentId == commentId);
            dbRepo.Comments.Remove(comment);
            dbRepo.SaveChanges();
            return RedirectToAction("CurrentPost", "Home", new { postId = post }); ;
        }

        public ActionResult AddPost(string username)
        {
            ViewBag.Person = userRepo.Users.First(u => u.Username == User.Identity.Name);
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
                dbRepo.Posts.Add(post);
                dbRepo.SaveChanges();
                return RedirectToAction("RecentPosts", "Home", new { username = User.Identity.Name.ToString() });
            }
            else
            {
                ViewBag.Person = userRepo.Users.First(u => u.Username == User.Identity.Name);
                return View();
            }
        }

        [HttpPost]
        public ActionResult DeletePost(int postId)
        {
            PostModel post = dbRepo.Posts.FirstOrDefault(p => p.PostId == postId);
            if (dbRepo.Comments.Count(c => c.PostId == postId) == 0)
            {
                dbRepo.Posts.Remove(post);
                dbRepo.SaveChanges();
                return RedirectToAction("Arhive", "Home", new { username = User.Identity.Name });
            }
            else
            {
                List<CommentModel> commentBase = dbRepo.Comments.Where(c => c.PostId == postId).ToList();
                foreach (var c in commentBase)
                {
                    dbRepo.Comments.Remove(c);
                }
                dbRepo.SaveChanges();
                dbRepo.Posts.Remove(post);
                dbRepo.SaveChanges();
                return RedirectToAction("Arhive", "Home", new { username = User.Identity.Name });
            }
        }

        [HttpPost]
        public ActionResult SearchResult(string searchtext)
        {
            SearchModel model = new SearchModel();
            model.Users = userRepo.Users.Where(u => u.FirstName == searchtext || u.LastName == searchtext);
            model.Post = postRepo.Posts.Where(p => p.Title == searchtext || p.PostText.Contains(searchtext));
            model.Comments = commentRepo.Comments.Where(p => p.CommentText.Contains(searchtext));
            return View(model);
        }
    }
}

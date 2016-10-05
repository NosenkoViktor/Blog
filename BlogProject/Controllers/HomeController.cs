using BlogProject.Abstract;
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
    public class HomeController : Controller
    {
        private EFUserRepository userRepo;
        private EFPostRepository postRepo;
        private EFCommentRepository commentRepo;
        private EFLikeRepository likeRepo;

        public HomeController(EFUserRepository userRepository, 
            EFPostRepository postRepository, 
            EFCommentRepository commentRepository, 
            EFDbContext dbRepository, EFLikeRepository likeRepository)
        {
            userRepo = userRepository;
            postRepo = postRepository;
            commentRepo = commentRepository;
            likeRepo = likeRepository;
        }

        public ActionResult RecentPosts(string username)
        {
            int id = GetUserId(username);
            ViewBag.Person = userRepo.Users.FirstOrDefault(u => u.ID == id);
            PostsViewModel viewModel = new PostsViewModel()
            {
                PostView = postRepo.Posts.Where(p => p.UserId == id).OrderByDescending(p => p.PostTime)
            };
            viewModel.Likes = likeRepo.Likes;
            return View(viewModel);
        }

        public FileContentResult GetImage(int id)
        {
            UserModel person = new UserModel() { User = userRepo.Users.FirstOrDefault(u => u.ID == id) };
            if (person != null)
                return File(person.User.ImageData, person.User.ImageMimeType);
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
            UserModel model = new UserModel();
            ViewBag.Person = userRepo.Users.FirstOrDefault(u => u.ID == id);
            foreach (var p in userRepo.Users)
            {
                if (id == p.ID) model.User = p;
            }
            return View(model);
        }

        [Authorize]
        public ActionResult ChangeInformation(string username)
        {
            int id = GetUserId(username);
            UserModel model = new UserModel();
            ViewBag.Person = userRepo.Users.FirstOrDefault(u => u.ID == id);
            foreach (var p in userRepo.Users)
            {
                if (id == p.ID) model.User = p;
            }
            return View(model.User);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangeInformation(Users user, HttpPostedFileBase image)
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
                userRepo.Edit(user);
                return RedirectToAction("Information", "Home", new { username = User.Identity.Name });
            }
            else
            {
                return View();
            }
        }

        public int GetUserId(string username)
        {
            UserModel userModel = new UserModel() { User = userRepo.Users.First(u => u.Username == username) };
            int id = userModel.User.ID;
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
            news.Likes = likeRepo.Likes;
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
            currentPost.Likes = likeRepo.Likes;
            return View(currentPost);
        }


        [HttpPost]
        public ActionResult AddComment(int post, CurrentPostModel newComment)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("LogIn", "Account"); 
            if (string.IsNullOrEmpty(newComment.UserComment.CommentText))
                return RedirectToAction("CurrentPost", "Home", new { postId = post });
            Comments comment = new Comments();
            comment.CommentText = newComment.UserComment.CommentText;
            comment.CommentTime = DateTime.Now;
            comment.PostId = post;
            comment.UserId = userRepo.Users.First(u => u.Username == User.Identity.Name).ID;
            commentRepo.Add(comment);
            return RedirectToAction("CurrentPost", "Home", new { postId = post });
        }

        [HttpPost]
        public ActionResult DeleteComment(int commentId, int post)
        {
            Comments comment = commentRepo.Comments.FirstOrDefault(c => c.CommentId == commentId);
            commentRepo.Delete(comment);
            return RedirectToAction("CurrentPost", "Home", new { postId = post }); ;
        }

        public ActionResult AddPost(string username)
        {
            ViewBag.Person = userRepo.Users.First(u => u.Username == User.Identity.Name);
            return View();
        }

        [HttpPost]
        public ActionResult AddPost(Posts newPost)
        {
            if (string.IsNullOrEmpty(newPost.Title) || string.IsNullOrEmpty(newPost.PostText))
            {
                ViewBag.Person = userRepo.Users.First(u => u.Username == User.Identity.Name);
                return View();
            }
            Posts post = new Posts();
            post.UserId = GetUserId(User.Identity.Name.ToString());
            post.Title = newPost.Title;
            post.PostText = newPost.PostText;
            post.PostTime = DateTime.Now;
            postRepo.Add(post);
            return RedirectToAction("RecentPosts", "Home", new { username = User.Identity.Name.ToString() });
        }

        [HttpPost]
        public ActionResult DeletePost(int postId)
        {
            Posts post = postRepo.Posts.FirstOrDefault(p => p.PostId == postId);
            if (commentRepo.Comments.Count(c => c.PostId == postId) == 0)
            {
                postRepo.Delete(post);
                return RedirectToAction("Arhive", "Home", new { username = User.Identity.Name });
            }
            else
            {
                List<Comments> commentBase = commentRepo.Comments.Where(c => c.PostId == postId).ToList();
                foreach (var c in commentBase)
                {
                    commentRepo.Delete(c);
                }
                postRepo.Delete(post);
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

        [HttpPost]
        [Authorize]
        public void PutLike(int postId, string returnUrl)
        {
            Likes like = new Likes();
            int id = GetUserId(User.Identity.Name);
            if (likeRepo.Likes.Count(u => u.PostID == postId && u.UserID == id) == 1)
            {
                like = likeRepo.Likes.First(u => u.PostID == postId);
                likeRepo.Delete(like);
            }
            else
            {
                like.PostID = postId;
                like.UserID = id;
                likeRepo.Add(like);
            }
            Response.Redirect(returnUrl);
        }
    }
}

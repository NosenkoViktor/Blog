using BlogProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    public class NewsViewModel
    {
        public IEnumerable<Posts> PostView { get; set; }
        public IEnumerable<Users> UserView { get; set; }
        public IEnumerable<Likes> Likes { get; set; }
    }
}
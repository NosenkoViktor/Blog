using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    public class PostsViewModel
    {
        public IEnumerable<Posts> PostView { get; set; }
        public IEnumerable<Users> Users { get; set; }
    }
}
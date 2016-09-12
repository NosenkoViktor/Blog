using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    public class PostsViewModel
    {
        public IEnumerable<PostModel> PostView { get; set; }
    }
}
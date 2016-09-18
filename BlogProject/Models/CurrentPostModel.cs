using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    public class CurrentPostModel
    {
        public PostModel Post { get; set; }
        public IEnumerable<CommentModel> Comments { get; set; }
        public CommentModel UserComment { get; set; }
        public IEnumerable<UserModel> Users { get; set; }
    }
}
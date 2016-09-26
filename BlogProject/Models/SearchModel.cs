using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    public class SearchModel
    {
        public IEnumerable<UserModel> Users { get; set; }
        public IEnumerable<PostModel> Post { get; set; }
        public IEnumerable<CommentModel> Comments { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    public class SearchModel
    {
        public IEnumerable<Users> Users { get; set; }
        public IEnumerable<Posts> Post { get; set; }
        public IEnumerable<Comments> Comments { get; set; }
    }
}
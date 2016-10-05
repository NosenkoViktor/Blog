using BlogProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    public class AdminModel
    {
        public Users User { get; set; }
        public IEnumerable<Comments> Comments { get; set; }
        public IEnumerable<Posts> Posts { get; set; }
    }
}
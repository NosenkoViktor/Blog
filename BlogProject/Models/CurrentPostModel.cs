using BlogProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    public class CurrentPostModel
    {
        public Posts Post { get; set; }
        public IEnumerable<Comments> Comments { get; set; }
        public Comments UserComment { get; set; }
        public IEnumerable<Users> Users { get; set; }
        public IQueryable<Likes> Likes { get; set; }
    }
}
using BlogProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    public class LikeModel
    {
        public IQueryable<Likes> Likes { get; set; }
    }
}
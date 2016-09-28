using BlogProject.Abstract;
using BlogProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Concrete
{
    public class EFPostRepository : IPostRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Posts> Posts
        {
            get { return context.Posts; }
        }
    }
}
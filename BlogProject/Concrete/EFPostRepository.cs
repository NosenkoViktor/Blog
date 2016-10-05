using BlogProject.Abstract;
using BlogProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Concrete
{
    public class EFPostRepository : GenericRepository<EFDbContext, Posts>
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Posts> Posts
        {
            get { return context.Posts; }
        }
    }
}
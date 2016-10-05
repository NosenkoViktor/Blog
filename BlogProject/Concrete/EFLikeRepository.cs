using BlogProject.Abstract;
using BlogProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Concrete
{
    public class EFLikeRepository : GenericRepository<EFDbContext, Likes>
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Likes> Likes
        {
            get { return context.Likes; }
        }
    }
}
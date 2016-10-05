using BlogProject.Abstract;
using BlogProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Concrete
{
    public class EFCommentRepository : GenericRepository<EFDbContext, Comments>
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Comments> Comments
        {
            get { return context.Comments; }
        }
    }
}
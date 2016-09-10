using BlogProject.Abstract;
using BlogProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Concrete
{
    public class EFCommentRepository : ICommentRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<CommentModel> Comments
        {
            get { return context.Comments; }
        }
    }
}
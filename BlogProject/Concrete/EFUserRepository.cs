﻿using BlogProject.Abstract;
using BlogProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Concrete
{
    public class EFUserRepository : GenericRepository<EFDbContext, Users>
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Users> Users
        {
            get { return context.Users; }
        }
    }
}
﻿using BlogProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Abstract
{
    public interface IPostRepository
    {
        IQueryable<PostModel> Posts { get; }
    }
}
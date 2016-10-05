using BlogProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    public class AdminViewModel
    {
        public IEnumerable<Users> Users { get; set; }
    }
}
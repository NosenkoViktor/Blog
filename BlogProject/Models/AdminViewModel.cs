using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    public class AdminViewModel
    {
        public IEnumerable<UserModel> Users { get; set; }
    }
}
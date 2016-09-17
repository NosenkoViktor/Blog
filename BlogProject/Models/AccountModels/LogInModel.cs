using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogProject.Models.AccountModels
{
    public class LogInModel
    {
        [Required(ErrorMessage = "Enter Username please")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter password please")]
        public string Password { get; set; }
    }
}
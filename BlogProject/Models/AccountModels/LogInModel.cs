using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogProject.Models.AccountModels
{
    public class LogInModel
    {
        [Required(ErrorMessage = "Enter USER NAME please")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter PASSWORD please")]
        public string Password { get; set; }
    }
}
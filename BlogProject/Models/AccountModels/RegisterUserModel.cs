using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogProject.Models.AccountModels
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage="Enter User name please")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter First name please")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter Last name please")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter E-mail name please")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Password name please")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Repeat password name please")]
        public string RepeatPassword { get; set; }
    }
}
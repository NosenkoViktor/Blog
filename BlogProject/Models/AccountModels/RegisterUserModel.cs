using BlogProject.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogProject.Models.AccountModels
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage="Enter User name please")]
        [Remote("CheckUserName", "Account")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter First name please")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter Last name please")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter E-mail name please")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The value of {0} must contain not less than {2} characters .", MinimumLength = 6)]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter Password name please")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Repeat password name please")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Password and confirmation do not match.")]
        public string RepeatPassword { get; set; }

    }
}
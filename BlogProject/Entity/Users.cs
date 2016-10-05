using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogProject.Entity
{
    [Table("Users")]
    public class Users
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Username { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter First name please")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter Last name please")]
        public string LastName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter correct age")]
        public int Age { get; set; }

        public string Fone { get; set; }

        [Required(ErrorMessage = "Enter E-mail please")]
        public string Email { get; set; }

        public string Skype { get; set; }

        public byte[] ImageData { get; set; }
        [HiddenInput(DisplayValue=false)]
        public string ImageMimeType { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Roles { get; set; }
    }
}
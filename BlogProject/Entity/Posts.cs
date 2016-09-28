using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogProject.Models
{
    [Table("Posts")]
    public class Posts
    {
        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        [Key]
        [HiddenInput(DisplayValue = false)]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Enter title for post please")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Enter post text please")]
        public string PostText { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime PostTime { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogProject.Entity
{
    [Table("Comments")]
    public class Comments
    {
        public int UserId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int PostId { get; set; }
        [Key]
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentTime { get; set; }
    }
}
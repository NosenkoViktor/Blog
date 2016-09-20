﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogProject.Models
{
    [Table("Comments")]
    public class CommentModel
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
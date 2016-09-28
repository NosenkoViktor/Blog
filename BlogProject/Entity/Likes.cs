using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BlogProject.Entity
{
    [Table("Likes")]
    public class Likes
    {
        [Key]
        public int LikesKey { get; set; }
        public int UserID { get; set; }
        public int PostID { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    [Table("PostTable")]
    public class PostModel
    {
        public int UserId { get; set; }
        [Key]
        public int PostId { get; set; }
        public string Title { get; set; }
        public string PostText { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }
        public DateTime PostTime { get; set; }
    }
}
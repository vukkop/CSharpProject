using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpProject.Models
{
    public class Post
    {
        [Key]
        public int PostId { get;set; }
        [Required(ErrorMessage = "Post is required")]
        public string MyPost { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public int UserId {get;set;}
        public User? Writer {get;set;}
        public List<Comment> CommentsOnPost {get;set;} = new List<Comment>();
    }
}
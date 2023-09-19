using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpProject.Models
{
    public class Comment
    {
        [Key]
        public int CommentId {get;set;}
        [Required(ErrorMessage = "Comment is required")]
        public string MyComment {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public int UserId {get;set;}
        public User? Commenter {get;set;}
        public int PostId {get;set;}
        public Post? PostCommentedOn {get;set;}
    }
}
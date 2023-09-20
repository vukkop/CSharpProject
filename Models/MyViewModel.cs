using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpProject.Models
{
    public class MyViewModel
    {
        public User LoggedInUser {get;set;}
        public Post NewPost {get;set;}
        public Comment NewComment {get;set;}
        public List<Post> AllPosts {get;set;}
    }
}
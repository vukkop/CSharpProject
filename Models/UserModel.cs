using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpProject.Models
{
  public class UserModel
  {
    [Key]
    public int UserId { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpProject.Models;

public class LoginUser
{
  [Required]
  [EmailAddress]
  public string EmailLogin { get; set; }
  [Required]
  [DataType(DataType.Password)]
  public string PasswordLogin { get; set; }
}


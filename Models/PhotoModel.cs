using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpProject.Models
{
  public class Photo
  {
    [Key]
    public int PhotoId { get; set; }
    public string? PhotoTitle { get; set; }
    public string PhotoUrl { get; set; }
    public string PublicId { get; set; }
    public bool IsMain { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public int UserId { get; set; }
    public User? User { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpProject.Models
{
  public class Message
  {
    [Key]
    public int MessageId { get; set; }
    public int SenderId { get; set; }
    public string SenderUsername { get; set; }
    public string SenderProfilePhoto { get; set; }
    public User Sender { get; set; }
    public int RecipientId { get; set; }
    public string RecipientUsername { get; set; }
    public string RecipientProfilePhoto { get; set; }
    public User Recipient { get; set; }
    public string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.Now;
    public bool SenderDeleted { get; set; } = false;
    public bool RecipientDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

  }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpProject.Models
{
  public class Friendship
  {
    [Key]
    public int FriendshipId { get; set; }
    public int UserSentRequestID { get; set; }
    public int UserRecievedRequestId { get; set; }
    public bool FriendRequestAccepted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
  }
}


using Microsoft.EntityFrameworkCore;

namespace CSharpProject.Models;

public class MyContext : DbContext
{
  public MyContext(DbContextOptions options) : base(options) { }
  public DbSet<User> Users { get; set; }
  public DbSet<Photo> Photos { get; set; }
  public DbSet<Friendship> Friendships { get; set; }
  public DbSet<Message> Messages { get; set; }
  public DbSet<Post> Posts { get; set; }
  public DbSet<Comment> Comments { get; set; }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<Message>()
      .HasOne(u => u.Recipient)
      .WithMany(m => m.MessagesReceived)
      .OnDelete(DeleteBehavior.Restrict);

    builder.Entity<Message>()
      .HasOne(u => u.Sender)
      .WithMany(m => m.MessagesSent)
      .OnDelete(DeleteBehavior.Restrict);
  }
}

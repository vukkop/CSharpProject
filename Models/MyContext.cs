
using Microsoft.EntityFrameworkCore;

namespace CSharpProject.Models;

public class MyContext : DbContext
{
  public MyContext(DbContextOptions options) : base(options) { }
  public DbSet<User> Users { get; set; }
  public DbSet<Photo> Photos { get; set; }
  public DbSet<Friendship> Friendships { get; set; }


}

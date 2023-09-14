
using Microsoft.EntityFrameworkCore;

namespace CSharpProject.Models;

public class MyContext : DbContext
{
  public MyContext(DbContextOptions options) : base(options) { }

  //   <Add Models here(EXAMPLE)> =>
  //   public DbSet<Chef> Chefs { get; set; }
  // public DbSet<Dish> Dishes { get; set; }

}

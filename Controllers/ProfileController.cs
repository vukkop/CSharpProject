using System.Diagnostics;
using CSharpProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace CSharpProject.Controllers;

public class ProfileController : Controller
{
  private readonly ILogger<ProfileController> _logger;
  private MyContext _context;
  public ProfileController(ILogger<ProfileController> logger, MyContext context)
  {
    _logger = logger;
    _context = context;
  }
  [SessionCheck]
  [HttpPost("profiles/{id}/update")]
  public IActionResult UpdateProfile(User newUser, int id)
  {
      System.Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
      System.Console.WriteLine(newUser.DateOfBirth);
      System.Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
    User? OldUser = _context.Users.FirstOrDefault(i => i.UserId == id);
    if (!ModelState.IsValid)
{
    var message = string.Join(" | ", ModelState.Values
        .SelectMany(v => v.Errors)
        .Select(e => e.ErrorMessage));
    Console.WriteLine(message);
}
    if(ModelState.IsValid)
    {
      System.Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
      System.Console.WriteLine(OldUser.FirstName);
      System.Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
      OldUser.FirstName = newUser.FirstName;
      OldUser.LastName = newUser.LastName;
      OldUser.UserName = newUser.UserName;
      // OldUser.Location = newUser.Location;
      OldUser.Occupation = newUser.Occupation;
      // OldUser.Gender = newUser.Gender;
      OldUser.RelationshipStatus = newUser.RelationshipStatus;
      OldUser.DateOfBirth = newUser.DateOfBirth;
      OldUser.UpdatedAt = DateTime.Now;
      _context.SaveChanges();
      return RedirectToAction("SingleProfile", new {id = OldUser.UserId} );
    }
    return View("../Profile/EditProfile", OldUser);
  }

  [HttpGet("profiles/{id}/edit")]
  public IActionResult EditProfile(int id)
  {
    User? UserToEdit = _context.Users.FirstOrDefault(e => e.UserId == id);
    return View("EditProfile", UserToEdit);
  }
  
  [HttpGet("profiles/{id}")]
  public IActionResult SingleProfile(int id)
  {
    User? User = _context.Users.FirstOrDefault(e => e.UserId == id);
    return View("SingleProfile", User);
  }

}
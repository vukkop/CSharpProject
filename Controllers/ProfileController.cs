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
    User? OldUser = _context.Users.FirstOrDefault(i => i.UserId == id);
    if(ModelState.IsValid)
    {
      OldUser.FirstName = newUser.FirstName;
      OldUser.LastName = newUser.LastName;
      OldUser.UserName = newUser.UserName;
      OldUser.Location = newUser.Location;
      OldUser.Occupation = newUser.Occupation;
      OldUser.Gender = newUser.Gender;
      OldUser.RelationshipStatus = newUser.RelationshipStatus;
      OldUser.DateOfBirth = newUser.DateOfBirth;
      OldUser.UpdatedAt = DateTime.Now;
      _context.SaveChanges();
      return RedirectToAction("SingleProfile", new {id = OldUser.UserId} );
    }
    return View("../Profile/SingleProfile", OldUser);
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
using System.Diagnostics;
using CSharpProject.Interfaces;
using CSharpProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace CSharpProject.Controllers;

[SessionCheck]
public class ProfileController : Controller
{
  private readonly ILogger<ProfileController> _logger;
  private MyContext _context;
  private readonly IPhotoService _photoService;
  public ProfileController(ILogger<ProfileController> logger, MyContext context, IPhotoService photoService)
  {
    _photoService = photoService;
    _logger = logger;
    _context = context;
  }
  [HttpPost("profiles/{id}/update")]
  public IActionResult UpdateProfile(EditUser newUser, int id)
  {
    User? OldUser = _context.Users.FirstOrDefault(i => i.UserId == id);
    if (!ModelState.IsValid)
    {
      var message = string.Join(" | ", ModelState.Values
          .SelectMany(v => v.Errors)
          .Select(e => e.ErrorMessage));
      Console.WriteLine(message);
    }
    if (ModelState.IsValid)
    {
      OldUser.FirstName = newUser.FirstName;
      OldUser.LastName = newUser.LastName;
      OldUser.UserName = newUser.UserName;
      // OldUser.Location = newUser.Location;
      OldUser.Occupation = newUser.Occupation;
      OldUser.Gender = newUser.Gender;
      OldUser.RelationshipStatus = newUser.RelationshipStatus;
      OldUser.Address = newUser.Address;
      OldUser.City = newUser.City;
      OldUser.State = newUser.State;
      OldUser.Country = newUser.Country;
      OldUser.DateOfBirth = newUser.DateOfBirth;
      OldUser.UpdatedAt = DateTime.Now;
      _context.SaveChanges();
      HttpContext.Session.SetString("UserName", newUser.UserName);
      return RedirectToAction("SingleProfile", new { id = OldUser.UserId });
    }
    return View("../Profile/EditProfile", OldUser);
  }

  [HttpGet("profiles/{id}/edit")]
  public IActionResult EditProfile(int id)
  {
    User? UserToEdit = _context.Users.Include(p => p.Photos).FirstOrDefault(e => e.UserId == id);
    return View("EditProfile", UserToEdit);
  }

  [HttpGet("profiles/{id}")]
  public IActionResult SingleProfile(int id)
  {
    User? User = _context.Users.Include(p => p.Photos).Include(e => e.MyPosts).ThenInclude(c => c.CommentsOnPost).ThenInclude(i => i.Commenter).OrderByDescending(a => a.CreatedAt).ToList().FirstOrDefault(e => e.UserId == id);
    return View("SingleProfile", User);
  }

  [HttpPost("photos/upload")]
  public async Task<IActionResult> AddPhoto(IFormFile file)
  {
    var user = _context.Users.Include(p => p.Photos).FirstOrDefault(e => e.UserId == HttpContext.Session.GetInt32("UserId"));

    var result = await _photoService.AddPhotoAsync(file);

    if (result.Error != null) return BadRequest(result.Error.Message);

    var photo = new Photo
    {
      PhotoUrl = result.SecureUrl.AbsoluteUri,
      PublicId = result.PublicId,
      UserId = user.UserId
    };

    if (user.ProfilePhoto == "~/assets/images/user/blank-profile-picture.jpg")
    {
      photo.IsMain = true;
      user.ProfilePhoto = photo.PhotoUrl;
      _photoService.UpdateMessageProfilePhoto(user.UserId, photo.PhotoUrl);
    }
    HttpContext.Session.SetString("UserPhoto", user.ProfilePhoto);

    user.Photos.Add(photo);
    _context.SaveChanges();

    return RedirectToAction("EditProfile", new { id = user.UserId });
  }

  [HttpPost("photos/{photoId}/delete")]
  public async Task<IActionResult> DeletePhoto(int photoId)
  {
    var user = _context.Users.Include(p => p.Photos).FirstOrDefault(e => e.UserId == HttpContext.Session.GetInt32("UserId"));

    var photo = user.Photos.FirstOrDefault(e => e.PhotoId == photoId);

    if (photo == null) return NotFound();

    if (photo.IsMain) return BadRequest("You cannot delete your profile photo");

    if (photo.PublicId != null)
    {
      var result = await _photoService.DeletePhoto(photo.PublicId);
      if (result.Error != null) return BadRequest(result.Error.Message);
    }

    user.Photos.Remove(photo);
    _context.SaveChanges();

    return RedirectToAction("EditProfile", new { id = user.UserId });
  }

  [HttpPost("photos/{photoId}/edit")]
  public async Task<IActionResult> ProfilePhoto(int photoId)
  {
    var user = _context.Users.Include(p => p.Photos).FirstOrDefault(e => e.UserId == HttpContext.Session.GetInt32("UserId"));

    var selectedPhoto = user.Photos.FirstOrDefault(e => e.PhotoId == photoId);

    var mainPhoto = user.Photos.FirstOrDefault(e => e.IsMain);

    if (selectedPhoto == null) return NotFound();

    if (mainPhoto == null)
    {
      selectedPhoto.IsMain = true;
    }

    if (selectedPhoto.PhotoId == mainPhoto?.PhotoId)
    {
      return BadRequest("Selected photo is set already set as profile photo");
    }
    else if (mainPhoto != null)
    {
      selectedPhoto.IsMain = true;
      mainPhoto.IsMain = false;
    }
    user.ProfilePhoto = selectedPhoto.PhotoUrl;
    _photoService.UpdateMessageProfilePhoto(user.UserId, selectedPhoto.PhotoUrl);
    HttpContext.Session.SetString("UserPhoto", user.ProfilePhoto);

    _context.SaveChanges();

    return RedirectToAction("EditProfile", new { id = user.UserId });
  }
}

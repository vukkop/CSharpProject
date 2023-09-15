using System.Diagnostics;
using CSharpProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace CSharpProject.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;
  private MyContext _context;
  public HomeController(ILogger<HomeController> logger, MyContext context)
  {
    _logger = logger;
    _context = context;
  }

  [HttpGet("")]
  public IActionResult Index()
  {
    HttpContext.Session.Clear();
    return View("Index");
  }

  [HttpPost("users/create")]
  public IActionResult CreateUser(User newUser)
  {
    if (ModelState.IsValid)
    {
      PasswordHasher<User> Hasher = new PasswordHasher<User>();
      newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
      _context.Add(newUser);
      _context.SaveChanges();
      HttpContext.Session.SetInt32("UserId", newUser.UserId);
      HttpContext.Session.SetString("UserName", newUser.UserName);
      HttpContext.Session.SetString("UserPhoto", newUser.ProfilePhoto);
      return RedirectToAction("Dashboard");
    }
    else
    {
      return View("Index");
    }
  }

  [HttpPost("users/login")]
  public IActionResult Login(LoginUser loginUser)
  {
    if (ModelState.IsValid)
    {
      User? userInDb = _context.Users.FirstOrDefault(e => e.Email == loginUser.EmailLogin);
      if (userInDb == null)
      {
        ModelState.AddModelError("Email", "Invalid Email/Password");
        return View("Index");
      }
      PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
      var result = hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.PasswordLogin);
      if (result == 0)
      {
        ModelState.AddModelError("Email", "Invalid Email/Password");
        return View("Index");
      }
      HttpContext.Session.SetInt32("UserId", userInDb.UserId);
      HttpContext.Session.SetString("UserName", userInDb.UserName);
      HttpContext.Session.SetString("UserPhoto", userInDb.ProfilePhoto);
      return RedirectToAction("Dashboard");
    }
    else
    {
      return View("Index");
    }
  }

  [HttpPost("users/logout")]
  public IActionResult Logout()
  {
    HttpContext.Session.Clear();
    return View("Index");
  }


  [SessionCheck]
  [HttpGet("users/dashboard")]
  public IActionResult Dashboard()
  {
    return View("Dashboard");
  }

  public IActionResult Privacy()
  {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}

public class SessionCheckAttribute : ActionFilterAttribute
{
  public override void OnActionExecuting(ActionExecutingContext context)
  {
    int? userId = context.HttpContext.Session.GetInt32("UserId");
    if (userId == null)
    {
      context.Result = new RedirectToActionResult("Index", "Home", null);
    }
  }
}

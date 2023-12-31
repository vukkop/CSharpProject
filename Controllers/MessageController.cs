using System.Diagnostics;
using System.Text.Json;
using CSharpProject.Interfaces;
using CSharpProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSharpProject.Controllers;

public class MessageController : Controller
{
  private readonly ILogger<MessageController> _logger;
  private MyContext _context;
  private readonly IMessageRepository _messageRepository;
  public MessageController(ILogger<MessageController> logger, MyContext context, IMessageRepository messageRepository)
  {
    _messageRepository = messageRepository;
    _logger = logger;
    _context = context;
  }

  [HttpGet("messages")]
  public IActionResult Messages()
  {
    var currentUserId = HttpContext.Session.GetInt32("UserId");
    ViewBag.Error = 0;
    MessagesViewModel MessagesViewModel = new MessagesViewModel()
    {
      Messages = _context.Messages
      .Include(s => s.Sender)
      .Include(r => r.Recipient)
      .Where(e =>
        e.SenderId == currentUserId || e.RecipientId == currentUserId).ToList(),
      AllUsers = _context.Users.Where(e => e.UserId != currentUserId).ToList()
    };

    return View("Messages", MessagesViewModel);
  }

  [HttpGet("messages/{userName}/thread")]
  public string MessageThread(string userName)
  {
    var currentUserName = HttpContext.Session.GetString("UserName");

    IEnumerable<Message> messages = _messageRepository.GetMessageThread(currentUserName, userName);

    string strJson = JsonSerializer.Serialize(messages);

    return strJson;
  }

  [HttpPost("messages/create")]
  public async Task<IActionResult> CreateMessage([FromBody] CreateMessage createMessage)
  {

    var username = HttpContext.Session.GetString("UserName");

    if (username.ToLower() == createMessage.RecipientUsername.ToLower())
    {
      return BadRequest("You cannot send messages to yourself");
    }

    var sender = _context.Users.FirstOrDefault(e => e.UserName == username);
    var recipient = _context.Users.FirstOrDefault(e => e.UserName == createMessage.RecipientUsername);

    if (recipient == null) return NotFound();

    var message = new Message
    {
      Sender = sender,
      Recipient = recipient,
      SenderId = sender.UserId,
      SenderUsername = sender.UserName,
      SenderProfilePhoto = sender.ProfilePhoto,
      RecipientId = recipient.UserId,
      RecipientUsername = recipient.UserName,
      RecipientProfilePhoto = recipient.ProfilePhoto,
      Content = createMessage.Content
    };

    _messageRepository.AddMessage(message);

    if (await _messageRepository.SaveAllAsync()) return Ok(new { message = "Message sent" });

    return BadRequest("Failed to send message");
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}



using System.Diagnostics;
using AutoMapper;
using CSharpProject.DTOs;
using CSharpProject.Helpers;
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
  public ActionResult<IAsyncEnumerable<MessageDto>> Messages()
  {
    var currentUserId = HttpContext.Session.GetInt32("UserId");

    List<Message> Chats = _context.Messages
      .Include(s => s.Sender)
      .Include(r => r.Recipient)
      .Where(e =>
        e.SenderId == currentUserId || e.RecipientId == currentUserId).ToList();

    return View("Messages", Chats);
  }

  // [HttpPost("messages/create")]
  // public async Task<ActionResult<MessageDto>> CreateMeassage(CreateMessageDto createMessageDto)
  // {
  //   var username = HttpContext.Session.GetString("UserName");

  //   if (username.ToLower() == createMessageDto.RecipientUsername.ToLower())
  //   {
  //     return BadRequest("You cannot send messages to yourself");
  //   }

  //   var sender = _context.Users.FirstOrDefault(e => e.UserName == username);
  //   var recipient = _context.Users.FirstOrDefault(e => e.UserName == createMessageDto.RecipientUsername);

  //   if (recipient == null) return NotFound();

  //   var message = new Message
  //   {
  //     Sender = sender,
  //     Recipient = recipient,
  //     SenderUsername = sender.UserName,
  //     RecipientUsername = recipient.UserName,
  //     Content = createMessageDto.Content
  //   };

  //   _messageRepository.AddMessage(message);

  //   if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

  //   return BadRequest("Failed to send message");
  // }

  // [HttpGet]
  // public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
  // {
  //   messageParams.Username = HttpContext.Session.GetString("UserName");
  //   var messages = await _messageRepository.GetMessagesForUser(messageParams);

  //   return messages;
  // }


  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }

  // [HttpGet("messages/{username}/thread")]
  // public ActionResult<IAsyncEnumerable<MessageDto>> GetMessageThread(string username)
  // {
  //   var currentUsername = HttpContext.Session.GetString("UserName");

  //   IEnumerable<Message> messageThread = _messageRepository.GetMessageThread(currentUsername, username);

  //   return View("Messages", messageThread);
  // }


}


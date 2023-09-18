
using CSharpProject.DTOs;
using CSharpProject.Helpers;
using CSharpProject.Interfaces;
using CSharpProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CSharpProject.Data
{
  public class MessageRepository : IMessageRepository
  {
    private readonly MyContext _context;

    public MessageRepository(MyContext context)
    {
      _context = context;
    }
    public void AddMessage(Message message)
    {
      _context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
      _context.Messages.Remove(message);
    }

    public async Task<Message> GetMessage(int id)
    {
      return await _context.Messages.FindAsync(id);
    }

    // public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
    // {
    //   var query = _context.Messages
    //     .OrderByDescending(x => x.MessageSent)
    //     .AsQueryable();

    //   query = messageParams.Container switch
    //   {
    //     "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username &&
    //       u.RecipientDeleted == false),
    //     "Outbox" => query.Where(u => u.Sender.UserName == messageParams.Username &&
    //         u.SenderDeleted == false),
    //     _ => query.Where(u => u.Recipient.UserName == messageParams.Username
    //         && u.RecipientDeleted == false && u.DateRead == null)
    //   };

    //   var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

    //   return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);

    // }

    public IEnumerable<Message> GetMessageThread(string currentUserName, string recipientUserName)
    {
      var query = _context.Messages
    .Where(
        m => m.RecipientUsername == currentUserName && m.RecipientDeleted == false &&
        m.SenderUsername == recipientUserName ||
        m.RecipientUsername == recipientUserName && m.SenderDeleted == false &&
        m.SenderUsername == currentUserName
    )
    .OrderBy(m => m.MessageSent)
    .AsQueryable();

      var unreadMessages = query.Where(m => m.DateRead == null
          && m.RecipientUsername == currentUserName).ToList();

      if (unreadMessages.Any())
      {
        foreach (var message in unreadMessages)
        {
          message.DateRead = DateTime.Now;
        }
      }

      return query.ToList();
    }

    public async Task<bool> SaveAllAsync()
    {
      return await _context.SaveChangesAsync() > 0;
    }
  }
}

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
        _context.SaveChangesAsync();
      }

      return query;
    }

    public async Task<bool> SaveAllAsync()
    {
      return await _context.SaveChangesAsync() > 0;
    }
  }
}

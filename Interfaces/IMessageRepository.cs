using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpProject.DTOs;
using CSharpProject.Helpers;
using CSharpProject.Models;

namespace CSharpProject.Interfaces
{
  public interface IMessageRepository
  {
    void AddMessage(Message message);
    void DeleteMessage(Message message);
    Task<Message> GetMessage(int id);
    // Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
    IEnumerable<Message> GetMessageThread(string currentUserName, string recipientUserName);
    Task<bool> SaveAllAsync();

  }
}

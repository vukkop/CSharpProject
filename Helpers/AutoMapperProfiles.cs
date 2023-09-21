using AutoMapper;
using CSharpProject.Models;

namespace CSharpProject.Helpers
{
  public class AutoMapperProfiles : Profile
  {
    // public AutoMapperProfiles()
    // {
    //   CreateMap<Message, MessageDto>()
    //       .ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.ProfilePhoto))
    //       .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(s => s.Recipient.ProfilePhoto));
    // }
  }
}

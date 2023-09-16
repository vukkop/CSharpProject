using CSharpProject.Data;
using CSharpProject.Interfaces;

namespace CSharpProject.Extensions
{
  public static class ApplicationServiceExtensions
  {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
    {
      services.AddScoped<IMessageRepository, MessageRepository>();

      return services;
    }
  }
}

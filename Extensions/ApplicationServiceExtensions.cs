using CSharpProject.Data;
using CSharpProject.Helpers;
using CSharpProject.Interfaces;
using CSharpProject.Services;

namespace CSharpProject.Extensions
{
  public static class ApplicationServiceExtensions
  {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
    {
      services.AddScoped<IMessageRepository, MessageRepository>();
      services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
      services.AddScoped<IPhotoService, PhotoService>();

      return services;
    }
  }
}

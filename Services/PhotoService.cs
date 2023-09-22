using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CSharpProject.Helpers;
using CSharpProject.Interfaces;
using CSharpProject.Models;
using Microsoft.Extensions.Options;

namespace CSharpProject.Services
{
  public class PhotoService : IPhotoService
  {
    private readonly Cloudinary _cloudinary;
    private readonly MyContext _context;
    public PhotoService(IOptions<CloudinarySettings> config, MyContext context)
    {
      var acc = new Account
      (
          config.Value.CloudName,
          config.Value.ApiKey,
          config.Value.ApiSecret
      );
      _cloudinary = new Cloudinary(acc);
      _context = context;

    }

    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
      var UploadResult = new ImageUploadResult();

      if (file.Length > 0)
      {
        using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
          File = new FileDescription(file.FileName, stream),
          Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
          Folder = "CSharpProject"
        };
        UploadResult = await _cloudinary.UploadAsync(uploadParams);
      }
      return UploadResult;
    }

    public async Task<DeletionResult> DeletePhoto(string publicId)
    {
      var deleteParams = new DeletionParams(publicId);

      return await _cloudinary.DestroyAsync(deleteParams);
    }
    public void UpdateMessageProfilePhoto(int userId, string newProfilePhotoUrl)
    {
      List<Message> senderMessages = _context.Messages.Where(e => e.SenderId == userId).ToList();
      foreach (var sMessage in senderMessages)
      {
        sMessage.SenderProfilePhoto = newProfilePhotoUrl;
      };
      List<Message> recipientMessages = _context.Messages.Where(e => e.RecipientId == userId).ToList();
      foreach (var rMessage in recipientMessages)
      {
        rMessage.RecipientProfilePhoto = newProfilePhotoUrl;
      };

    }
  }

}

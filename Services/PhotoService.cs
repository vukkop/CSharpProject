using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CSharpProject.Helpers;
using CSharpProject.Interfaces;
using Microsoft.Extensions.Options;

namespace CSharpProject.Services
{
  public class PhotoService : IPhotoService
  {
    private readonly Cloudinary _cloudinary;
    public PhotoService(IOptions<CloudinarySettings> config)
    {
      var acc = new Account
      (
          config.Value.CloudName,
          config.Value.ApiKey,
          config.Value.ApiSecret
      );

      _cloudinary = new Cloudinary(acc);
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
  }
}

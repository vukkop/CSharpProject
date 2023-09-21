using CloudinaryDotNet.Actions;

namespace CSharpProject.Interfaces
{
  public interface IPhotoService
  {
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
    Task<DeletionResult> DeletePhoto(string publicId);

  }
}

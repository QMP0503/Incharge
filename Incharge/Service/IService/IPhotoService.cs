using CloudinaryDotNet.Actions;

namespace Incharge.Service.IService
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file); // <ImageUploadResult>
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}

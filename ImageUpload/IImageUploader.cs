namespace GameStore.API.ImageUpload;

public interface IImageUploader
{
    Task<string> UploadImageAsync(IFormFile file);
}

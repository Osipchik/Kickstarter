using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Company.CloudStorage
{
    public interface ICloudStorage
    {
        Task<string> UploadFileAsync(IFormFile imageFile, string filename);

        Task DeleteFileAsync(string imageUrl);
        
        string CreateFileName(IFormFile imageFile, string userId);
    }
}
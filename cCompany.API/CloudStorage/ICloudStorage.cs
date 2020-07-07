﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Company.API.CloudStorage
{
    public interface ICloudStorage
    {
        Task<string> UploadFileAsync(IFormFile imageFile, string filename);
        
        Task DeleteFileAsync(string imageUrl);

        bool IsFileValid(IFormFile imageFile);

        string CreateFileName(IFormFile imageFile, string userId);
    }
}
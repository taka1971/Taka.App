using Microsoft.Extensions.Configuration;
using Taka.App.Deliverer.Domain.Exceptions;
using Taka.App.Deliverer.Domain.Interfaces;

namespace Taka.App.Deliverer.Application.Services
{
    public class LocalFileStorageService : IStorageService
    {
        private readonly IConfiguration _configuration;
        public LocalFileStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadFileAsync(byte[] fileData, string fileName)
        {
            var storage = Directory.Exists(_configuration["LocalStorage"] ?? throw new AppException("LocalStorage not configuration"));

            var localStorage =  storage ? _configuration["LocalStorage"] : Path.GetTempPath();
            var filePath = Path.Combine(localStorage, fileName);
            await File.WriteAllBytesAsync(filePath, fileData);
                        
            return filePath;
        }
    }
}

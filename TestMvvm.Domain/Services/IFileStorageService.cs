using Microsoft.AspNetCore.Http;

namespace TestMvvm.Domain.Services
{
    public interface IFileStorageService
    {
        public Task<string> UploadAsync(IFormFile file);
    }
}

using Microsoft.AspNetCore.Http;
using TestMvvm.Domain.Services;

namespace TestMvvm.Infrastructure.Service
{
    public class LocalFileStorageService : IFileStorageService
    {
        public async Task<string> UploadAsync(IFormFile file)
        {
            string dbfilepath = null;
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot", "Images");
            bool exists = Directory.Exists(pathToSave);
            if (!exists)
            {
                Directory.CreateDirectory(pathToSave);
            }

            if (file.Length > 0)
            {
                string fileName = file.FileName.Trim('"');
                fileName = fileName.Replace(" ", "-");
                string FileName = Guid.NewGuid().ToString() + fileName;
                dbfilepath = Path.Combine("Images", FileName);
                using (var fileStream = new FileStream(Path.Combine(pathToSave, FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            return dbfilepath;
        }
    }
}

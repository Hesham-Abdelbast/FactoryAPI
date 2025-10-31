using Application.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Implementation
{
    public class FileServices : IFileServices
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileServices> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public FileServices(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, ILogger<FileServices> logger, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                // Validate input
                if (file == null || file.Length == 0)
                    throw new ArgumentException("File is null or empty", nameof(file));

                // Get configuration values
                var folderName = _configuration["UploadSettings:UploadFolder"];
                if (string.IsNullOrWhiteSpace(folderName))
                    throw new ArgumentException("Upload folder configuration is missing");

                // Get configuration values
                var requestPath = _configuration["UploadSettings:RequestPath"];
                if (string.IsNullOrWhiteSpace(requestPath))
                    throw new ArgumentException("RequestPath configuration is missing");

                // Create secure file path
                var uploadsPath = Path.Combine(Directory.GetParent(_webHostEnvironment.ContentRootPath)!.Parent!.FullName, folderName);
                var sanitizedFileName = Path.GetFileName(file.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}_{sanitizedFileName}";
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                // Ensure directory exists
                Directory.CreateDirectory(uploadsPath);

                // Save file asynchronously
                using var fileStream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(fileStream);

                var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                                  $"{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}" +
                                  $"{requestPath}/{uniqueFileName}";
                return urlFilePath;
            }
            catch (Exception ex) when (
            ex is ArgumentException ||
            ex is IOException ||
                ex is UnauthorizedAccessException)
            {
                // Log exception here (implementation omitted)
                _logger.LogError(ex, "Error uploading file: {FileName}", file.FileName);
                throw new Exception($"Error uploading file: {ex.Message}", ex);
            }
        }

        public Task<bool> DeleteFile(string filePath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            catch (Exception ex) when (
                ex is IOException ||
                ex is UnauthorizedAccessException)
            {
                // Log exception here (implementation omitted)
                _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);

                return Task.FromResult(false);
            }
        }
    }
}
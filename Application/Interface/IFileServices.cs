using Microsoft.AspNetCore.Http;
using System.IO;

namespace Application.Interface
{
    public interface IFileServices
    {
        /// <summary>
        /// Uploads a file to the specified path.
        /// </summary>
        /// <param name="file">The file to be uploaded.</param>
        /// <returns>True if the upload was successful, otherwise false.</returns>
        Task<string> UploadFile(IFormFile file);
        /// <summary>
        /// Deletes a file from the specified path.
        /// </summary>
        /// <param name="filePath">The path of the file to be deleted.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        Task<bool> DeleteFile(string filePath);
    }
}

using FileUpload.Entities;

namespace FileUpload.Services
{
    public interface IFileService
    {
        public Task PostFileAsync(IFormFile fileData, FileType fileType);
        public Task PostMultiFileAsync(List<FileUploadModel> FileData);
        public Task DownloadFileById(int fileaName);
    }
}

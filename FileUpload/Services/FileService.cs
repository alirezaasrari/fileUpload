using FileUpload.Context;
using FileUpload.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileUpload.Services
{
    public class FileService : IFileService
    {
        private readonly DataContext _datacontext;

        public FileService(DataContext datacontext)
        {
            _datacontext = datacontext;
        }

        public async Task DownloadFileById(int Id)
        {
            try
            {
                var file = _datacontext.FileDetails.Where(x => x.Id == Id).FirstOrDefaultAsync();

                var content = new System.IO.MemoryStream(file.Result.FileData);
                var path = Path.Combine(
                   Directory.GetCurrentDirectory(), "FileDownloaded",
                   file.Result.FileName);

                await CopyStream(content, path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CopyStream(Stream stream, string downloadPath)
        {
            using var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fileStream);
        }

        public async Task PostFileAsync(IFormFile fileData, FileType fileType)
        {
            try
            {
                var fileDetails = new FileDetails()
                {
                  Id = 0,
                  FileName = fileData.FileName,
                  FileType = fileType,
                };

                using (var stream = new MemoryStream())
                {
                    fileData.CopyTo(stream);
                    fileDetails.FileData = stream.ToArray();
                }

                var result = _datacontext.FileDetails.Add(fileDetails);
                await _datacontext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task PostMultiFileAsync(List<FileUploadModel> FileData)
        {
            try
            {
                foreach(FileUploadModel file in FileData)
                {
                    var fileDetails = new FileDetails()
                    {
                        Id = 0,
                        FileName = file.FileDetails.FileName,
                        FileType = file.FileType,
                    };

                    using (var stream = new MemoryStream())
                    {
                        file.FileDetails.CopyTo(stream);
                        fileDetails.FileData = stream.ToArray();
                    }

                    var result = _datacontext.FileDetails.Add(fileDetails);
                }
                await _datacontext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

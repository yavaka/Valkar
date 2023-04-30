namespace ApplicationCore.Services.GoogleDriveAPI
{
    using ApplicationCore.Config;
    using ApplicationCore.ServiceModels.GoogleDriveAPI;
    using Google.Apis.Drive.v3;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class GoogleDriveAPIService : IGoogleDriveAPIService
    {
        private readonly DriveService _driveService;
        private readonly string _mainFolderId;
        public GoogleDriveAPIService(
            DriveService driveService,
            IOptions<ApplicationCoreOptions> options)
        {
            this._driveService = driveService;
            this._mainFolderId = options.Value.GoogleDriveMainFolderId;
        }

        #region Folders

        public async Task<string> CreateFolder(string folderName)
        {
            // Create a new folder object
            var folder = new Google.Apis.Drive.v3.Data.File
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string> { this._mainFolderId }
            };

            // Call the Files.Create method to create the folder
            var request = this._driveService.Files.Create(folder);
            request.Fields = "id";
            var createdFolder = await request.ExecuteAsync();

            return createdFolder.Id;
        }

        public IDictionary<string, string> GetFoldersByMainFolderId()
        {
            var request = this._driveService.Files.List();
            request.Q = $"'{this._mainFolderId}' in parents and mimeType = 'application/vnd.google-apps.folder'";
            request.Fields = "nextPageToken, files(id, name)";

            var results = new Dictionary<string, string>();
            do
            {
                var files = request.Execute();
                foreach (var file in files.Files)
                    results.Add(file.Id, file.Name);
                request.PageToken = files.NextPageToken;

            } while (!string.IsNullOrEmpty(request.PageToken));

            return results.OrderBy(v => v.Value)
              .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        #endregion

        #region Files

        public async Task<IEnumerable<GoogleDriveFileServiceModel>> GetFilesByFolderIdAsync(string folderId)
        {
            var request = this._driveService.Files.List();
            request.Q = $"'{folderId}' in parents and trashed = false and mimeType != 'application/vnd.google-apps.folder'";
            request.Fields = "nextPageToken, files(id, name, createdTime, modifiedTime, fileExtension)";

            var result = new List<GoogleDriveFileServiceModel>();
            do
            {
                var files = await request.ExecuteAsync();
                result.AddRange(files.Files
                    .Select(f => new GoogleDriveFileServiceModel
                    {
                        Id = f.Id,
                        Name = f.Name,
                        MimeType = f.FileExtension
                    }));
                request.PageToken = files.NextPageToken;

            } while (!string.IsNullOrEmpty(request.PageToken));

            return result;
        }

        public async Task UploadFile(IFormFile file, string fileName, string folderId)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = string.IsNullOrEmpty(fileName)
                    ? Path.GetFileName(file.FileName)
                    : fileName,
                Parents = new List<string> { folderId }
            };

            using var stream = new MemoryStream();
            file.CopyTo(stream);

            var request = this._driveService.Files.Create(fileMetadata, stream, GetMimeType(file.FileName));
            request.Fields = "id, name, createdTime, size, mimeType";

            await request.UploadAsync();
        }

        public async Task<GoogleDriveFileServiceModel> DownloadFileById(string fileId)
        {
            var file = await this._driveService.Files.Get(fileId).ExecuteAsync();
            var result = new GoogleDriveFileServiceModel
            {
                Name = file.Name,
            };

            using var stream = new MemoryStream();

            if (file.MimeType == "application/vnd.google-apps.document")
            {
                result.MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

                var exportRequest = _driveService.Files.Export(fileId, result.MimeType);

                await exportRequest.DownloadAsync(stream);

                result.Name += ".docx";
            }
            else
            {
                await this._driveService.Files.Get(fileId).DownloadAsync(stream);
                result.MimeType = file.MimeType;
                result.Name += GetFileExtension(file.MimeType);
            }

            stream.Position = 0;
            result.Data = stream.ToArray();

            return result;
        }

        #endregion

        #region Helpers

        // Helper method to get the MIME type of a file based on its extension.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLowerInvariant();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext)!;
            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type")!.ToString()!;
            }
            return mimeType;
        }

        private static string GetFileExtension(string mimeType)
            => mimeType switch
            {
                "application/msword" => ".doc",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => ".docx",
                "application/vnd.ms-excel" => ".xls",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => ".xlsx",
                "application/vnd.ms-powerpoint" => ".ppt",
                "application/vnd.openxmlformats-officedocument.presentationml.presentation" => ".pptx",
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "image/gif" => ".gif",
                "image/webp" => ".webp",
                "text/plain" => ".txt",
                "application/pdf" => ".pdf",
                "application/zip" => ".zip",
                _ => ""
            };

        #endregion
    }
}

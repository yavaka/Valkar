namespace ApplicationCore.Services.GoogleDriveAPI
{
    using ApplicationCore.ServiceModels.GoogleDriveAPI;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGoogleDriveAPIService
    {
        #region Folders

        Task<string> CreateFolder(string folderName);
        IDictionary<string, string> GetFoldersByMainFolderId();

        #endregion

        #region Files


        Task<IEnumerable<GoogleDriveFileServiceModel>> GetFilesByFolderIdAsync(string folderId);

        Task UploadFile(IFormFile file, string fileName, string folderId);

        Task<GoogleDriveFileServiceModel> DownloadFileById(string fileId);

        #endregion
    }
}

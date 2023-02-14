namespace ApplicationCore.Services.File
{
    using ApplicationCore.ServiceModels.Document;
    using Infrastructure.Common.Enums;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFileService
    {
        Task<ICollection<File>> ProcessEmployeeUploadedDocuments(EmployeeDocumentsServiceModel documents);
        IEnumerable<DownloadDocumentServiceModel> GetEmployeeDocuments(string uploaderId, EmployeeDocumentTypes documentType);
        /// <summary>
        /// Get zip file consisting multiple documents
        /// </summary>
        /// <returns></returns>
        Task<DownloadDocumentServiceModel> GetZipFile(List<DownloadDocumentServiceModel> documents);
        Task<TempDocumentServiceModel> ProcessDocument(IFormFile formFile);
    }
}

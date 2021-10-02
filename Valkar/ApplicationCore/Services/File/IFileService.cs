namespace ApplicationCore.Services.File
{
    using ApplicationCore.ServiceModels.Document;
    using Infrastructure.Common.Enums;
    using Infrastructure.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFileService
    {
        Task<ICollection<File>> ProcessUploadedDocuments(DocumentsServiceModel documents);
        IEnumerable<DownloadDocumentServiceModel> GetDocuments(string uploaderId, DocumentTypes documentType);
        /// <summary>
        /// Get zip file consisting multiple documents
        /// </summary>
        /// <returns></returns>
        Task<DownloadDocumentServiceModel> GetZipFile(List<DownloadDocumentServiceModel> documents);
    }
}

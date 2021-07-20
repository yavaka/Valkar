namespace ApplicationCore.Services.File
{
    using ApplicationCore.ServiceModels.Document;
    using Infrastructure.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFileService
    {
        public Task<ICollection<File>> ProcessUploadedDocuments(DocumentsServiceModel documents);
    }
}

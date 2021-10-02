namespace Web.Controllers
{
    using ApplicationCore.Services.File;
    using Infrastructure.Common.Enums;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;

    public class DocumentsController : Controller
    {
        private readonly IFileService _fileService;

        public DocumentsController(IFileService fileService)
            => this._fileService = fileService;

        public async Task<FileResult> DownloadAsync(string uploaderId, DocumentTypes documentType)
        {
            // Get the documents
            var documents = this._fileService
                .GetDocuments(uploaderId, documentType);

            // Download one document
            if (documents.ToList().Count == 1)
            {
                var doc = documents.First();
                return File(doc.Data, doc.FileType, $"{doc.Name}{doc.Extension}");
            }

            // Download multiple documents in a zip file
            var zipFile = await this._fileService.GetZipFile(documents.ToList());
            return File(zipFile.Data, zipFile.FileType, $"{zipFile.Name}{zipFile.Extension}");
        }
    }
}

namespace ApplicationCore.Services.PDFDocument
{
    using ApplicationCore.ServiceModels.Document;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDocumentService
    {
        Task SendDocumentToEmployee(TempDocumentServiceModel model);
        Task UploadSignedDocument(TempDocumentServiceModel model);
        Task<ICollection<TempDocumentServiceModel>> FetchAllDocuments();
        Task<ICollection<TempDocumentServiceModel>> FetchAllDocumentsByEmployeeId(string employeeId);
        Task<TempDocumentServiceModel> GetDocumentByIdAsync(string id);
        Task DeleteAsync(string id);
    }
}

namespace ApplicationCore.Services.PDFDocument
{
    using ApplicationCore.ServiceModels.Document;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDocumentService
    {
        Task SendDocumentToEmployee(TempDocumentServiceModel model);
        Task<ICollection<TempDocumentServiceModel>> FetchDocuments();
        Task<TempDocumentServiceModel> GetDocumentByIdAsync(string id);
        Task DeleteAsync(string id);
    }
}

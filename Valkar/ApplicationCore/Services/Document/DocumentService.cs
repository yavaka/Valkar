namespace ApplicationCore.Services.PDFDocument
{
    using ApplicationCore.ServiceModels.Document;
    using ApplicationCore.ServiceModels.Driver;
    using ApplicationCore.Services.Mapper;
    using Infrastructure;
    using Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DriverModel = Infrastructure.Models.Driver;

    public class DocumentService : IDocumentService
    {
        private readonly ValkarDbContext _data;
        private readonly IMapperService _mapper;

        public DocumentService(ValkarDbContext data,
            IMapperService mapper)
        {
            this._data = data;
            this._mapper = mapper;
        }

        /// <summary>
        /// This method save uploaded PDF to Sign PDF PersonalDocuments table.
        /// </summary>
        public async Task SendDocumentToEmployee(TempDocumentServiceModel model)
        {
            await this._data.AddAsync(this._mapper.Map<TempDocumentServiceModel, TempDocument>(model));
            await this._data.SaveChangesAsync();
        }

        public async Task<ICollection<TempDocumentServiceModel>> FetchDocuments()
        {
            // Automapper does not map the employee details
            var documentsModel = await this._data.TempDocuments
                .Include(s => s.SentTo)
                .ToListAsync();

            var result = new List<TempDocumentServiceModel>();

            documentsModel.ForEach(doc => result.Add(MapToServiceModel(doc)));

            return result;
        }

        public async Task<TempDocumentServiceModel> GetDocumentByIdAsync(string id)
            => MapToServiceModel(await FetchByIdAsync(id));

        public async Task DeleteAsync(string id)
        {
            this._data.TempDocuments.Remove(await FetchByIdAsync(id));
            await this._data.SaveChangesAsync();
        }

        #region Mapper

        private TempDocumentServiceModel MapToServiceModel(TempDocument model)
        {
            if (model is null)
                return null;

            var serviceModel = this._mapper.Map<TempDocument, TempDocumentServiceModel>(model);
            serviceModel.SentTo = this._mapper.Map<DriverModel, DriverDetailsServiceModel>(model.SentTo);

            return serviceModel;
        }

        #endregion

        #region Helpers

        private async Task<TempDocument> FetchByIdAsync(string id)
            => await this._data.TempDocuments
                .Include(s => s.SentTo)
                .FirstOrDefaultAsync(p => p.Id.ToString() == id);

        #endregion

    }
}

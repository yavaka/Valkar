namespace ApplicationCore.Services.Company
{
    using ApplicationCore.ServiceModels.Company;
    using ApplicationCore.Services.Mapper;
    using Infrastructure;
    using Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CompanyService : ICompanyService
    {
        private readonly ValkarDbContext _data;
        private readonly IMapperService _mapper;

        public CompanyService(ValkarDbContext data,
            IMapperService mapper)
        {
            this._data = data;
            this._mapper = mapper;
        }

        public async Task AddAsync(CompanyServiceModel model)
        {
            await this._data.AddAsync(this._mapper.Map<CompanyServiceModel, Company>(model));
            await this._data.SaveChangesAsync();
        }

        public async Task<List<CompanyServiceModel>> FetchAllAsync()
            => await this._data.PartnerCompanies
                .Select(c => this._mapper.Map<Company, CompanyServiceModel>(c))
                .ToListAsync();

        public async Task<CompanyServiceModel> FetchAsync(string id)
            => await this._data.PartnerCompanies
                .Where(i => i.Id.ToString() == id)
                .Select(c => this._mapper.Map<Company, CompanyServiceModel>(c))
                .FirstOrDefaultAsync();

        public async Task UpdateAsync(CompanyServiceModel model)
        {
            this._data
                .Entry(await FetchByIdAsync(model.Id))
                .CurrentValues
                .SetValues(this._mapper.Map<CompanyServiceModel, Company>(model));

            await this._data.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            this._data.PartnerCompanies.Remove(await FetchByIdAsync(id));
            await this._data.SaveChangesAsync();
        }

        #region Helpers

        private async Task<Company> FetchByIdAsync(string id)
            => await this._data.PartnerCompanies
                .FirstOrDefaultAsync(i => i.Id.ToString() == id);
        
        #endregion
    }
}

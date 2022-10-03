namespace ApplicationCore.Services.Company
{
    using ApplicationCore.ServiceModels.Company;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICompanyService
    {
        Task<List<CompanyServiceModel>> FetchAllAsync();
        Task<CompanyServiceModel> FetchAsync(string id);
        Task UpdateAsync(CompanyServiceModel model);
        Task AddAsync(CompanyServiceModel model);
        Task DeleteAsync(string id);
    }
}

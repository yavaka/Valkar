namespace ApplicationCore.Services.Driver
{
    using ApplicationCore.ServiceModels.Driver;
    using System.Threading.Tasks;

    public interface IDriverService
    {
        Task AddDriver(DriverDetailsServiceModel model, string userId);
    }
}

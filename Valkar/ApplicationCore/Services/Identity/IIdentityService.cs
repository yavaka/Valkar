namespace ApplicationCore.Services.Identity
{
    using ApplicationCore.ServiceModels.Identity;
    using System.Threading.Tasks;

    public interface IIdentityService
    {
        Task Register(RegisterServiceModel model);
        
        Task Login(LoginServiceModel model);

        Task Logout();
    }
}

namespace ApplicationCore.Services.Identity
{
    using ApplicationCore.ServiceModels.Identity;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IIdentityService
    {
        Task Register(RegisterServiceModel model);

        Task<bool> Login(LoginServiceModel model);

        Task Logout();

        string GetUserId(ClaimsPrincipal claimsPrincipal);

        Task CompleteOnboarding(string userId);
    }
}

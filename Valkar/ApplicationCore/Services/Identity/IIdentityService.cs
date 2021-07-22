namespace ApplicationCore.Services.Identity
{
    using ApplicationCore.ServiceModels.Identity;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IIdentityService
    {
        Task Register(RegisterServiceModel model);

        Task<bool> Login(LoginServiceModel model);

        Task Logout();

        string GetUserId(ClaimsPrincipal claimsPrincipal);

        Task CompleteOnboarding(string userId);

        Task<User> GetUserByEmail(string email);

        Task<string> GeneratePasswordResetToken(User user);

        Task<IdentityResult> ResetPassword(User user, string token, string newPassword);
    }
}

namespace ApplicationCore.Services.Identity
{
    using ApplicationCore.ServiceModels.Identity;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IIdentityService
    {
        Task Register(RegisterServiceModel model);

        Task<bool> Login(LoginServiceModel model);

        Task Logout();

        IEnumerable<User> GetAllUsers(); 

        string GetUserId(ClaimsPrincipal claimsPrincipal);

        Task<User> GetUserByEmail(string email);

        Task<string> GeneratePasswordResetToken(User user);

        Task<IdentityResult> ResetPassword(User user, string token, string newPassword);
        
        Task<IdentityResult> ChangePassword(string newPassword, ClaimsPrincipal claimsPrincipal);
        
        Task CompleteOnboarding(string userId);

        Task<bool> IsOnboardingCompleted(string userId);

        Task<bool> IsAdminLoggedIn(string email);

        Task<bool> IsOldPasswordValid(string oldPassword, ClaimsPrincipal claimsPrincipal);
       
        User GetUserById(string userId);
    }
}

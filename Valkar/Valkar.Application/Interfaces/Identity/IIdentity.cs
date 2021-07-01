namespace Valkar.Application.Interfaces.Identity
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Valkar.Application.ViewModels.Identity;

    public interface IIdentity
    {
        Task<bool> Register(RegisterViewModel model);

        Task<bool> Login(LoginViewModel model);

        IEnumerable<IUser> GetUsers();
    }
}

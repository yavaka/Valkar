namespace Valkar.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Valkar.Application.Interfaces.Identity;
    using Valkar.Application.ViewModels.Identity;
    using Valkar.Infrastructure.Identity;

    public class IdentityController : BaseApiController
    {
        private readonly IIdentity _identity;

        public IdentityController(IIdentity identity)
        {
            this._identity = identity;
        }

        [HttpGet]
        [Authorize]
        [Route("users")]
        public List<User>? GetUsers()
            => this._identity.GetUsers() as List<User>;

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
            => await this._identity.Register(model)
                ? Ok("Created user...")
                : BadRequest();

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model) 
            => await this._identity.Login(model)
                ? Ok("Successful login...")
                : BadRequest();
    }
}

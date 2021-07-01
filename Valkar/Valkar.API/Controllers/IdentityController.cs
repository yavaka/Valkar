namespace Valkar.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Valkar.Application.Interfaces.Identity;
    using Valkar.Application.ViewModels.Identity;
    using Valkar.Infrastructure.Identity;

    public class IdentityController : BaseApiController
    {
        private readonly IIdentity _identity;
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(
            IIdentity identity, 
            ILogger<IdentityController> logger) 
        {
            this._identity = identity;
            this._logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
            => await this._identity.Register(model)
                ? Ok("Created user...")
                : BadRequest();

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model) 
            => await this._identity.Login(model)
                ? Ok("Successful login...")
                : BadRequest();

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
            => await this._identity.Logout()
                ? Ok("User logged out...")
                : BadRequest();


        #region This method should be moved in admin controller
        [HttpGet]
        [Route("users")]
        public ActionResult<List<User>> GetUsers()
        {
            // Check is there authenticated user
            if (User is null || !User.Identity!.IsAuthenticated)
            {
                this._logger.LogError("User is not authenticated to resolve this request.");
                return Unauthorized();
            }

            // Get all users
            var result = this._identity.GetUsers() as List<User>;
            if (result is null)
            {
                return NoContent();
            }
            return result is null
                ? NoContent()
                : result;
        }
        #endregion
    }
}

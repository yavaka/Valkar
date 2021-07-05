namespace Web.Controllers
{
    using ApplicationCore.ServiceModels.Identity;
    using ApplicationCore.Services.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
            => this._identityService = identityService;

        [HttpGet]
        public IActionResult Register()
            => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterServiceModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await this._identityService.Register(model);

                    return RedirectToAction("Login");
                }
                catch (Exception e)
                {
                    // Inner exception message is the type name
                    ModelState.AddModelError(e.InnerException.Message, e.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
            => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginServiceModel model) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await this._identityService.Login(model);

                    return RedirectToAction("Privacy", "Home");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Login", e.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LogoutAsync() 
        {
            await this._identityService.Logout();

            return RedirectToAction("Index", "Home");
        }
    }
}

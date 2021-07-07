namespace Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ApplicationCore.Helpers;
    using ApplicationCore.ServiceModels.Identity;
    using ApplicationCore.Services.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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
                // Register - identity serivce
                await this._identityService.Register(model);

                // Success
                if (ModelErrorHelper.ModelErrors.Count is 0)
                {
                    return RedirectToAction("Login");
                }

                // Errors
                foreach (var error in ModelErrorHelper.ModelErrors)
                {
                    ModelState.AddModelError(error.Split(' ')[0], error);
                }
                ModelErrorHelper.ModelErrors = new List<string>();

                return View(model);
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
        public async Task<IActionResult> Logout()
        {
            await this._identityService.Logout();

            return RedirectToAction("Index", "Home");
        }
    }
}

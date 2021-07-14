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
                    ModelState.AddModelError(string.Empty, error);
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
                    // Whether or not the driver details completed
                    var isCompleted = await this._identityService.Login(model);

                    // Redirect to driver profile
                    if (isCompleted)
                    {
                        return RedirectToAction("Profile", "Drivers");
                    }
                    // Redirect to driver details form
                    return RedirectToAction("DriverDetails", "Drivers");
                }
                catch (Exception e)
                {
                    // Add model error from sign in manager
                    if (ModelErrorHelper.ModelErrors.Count is 0)
                    {
                        ModelState.AddModelError("Login", e.Message);
                    }
                    // Add model error from identity service login method
                    else
                    {
                        foreach (var error in ModelErrorHelper.ModelErrors)
                        {
                            ModelState.AddModelError(string.Empty, error);
                        }
                        ModelErrorHelper.ModelErrors = new List<string>();
                    }
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

namespace Web.Controllers
{
    using ApplicationCore.Enums;
    using ApplicationCore.Helpers;
    using ApplicationCore.ServiceModels.ContactUsForm;
    using ApplicationCore.Services.Email;
    using ApplicationCore.Services.Google.ReCaptcha;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Web.ViewModels;
    using static Infrastructure.Common.ModelConstants;

    public class HomeController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IGoogleReCaptchaService _googleReCaptchaService;

        public HomeController(IEmailService emailService, IGoogleReCaptchaService googleReCaptchaService)
        {
            this._emailService = emailService;
            this._googleReCaptchaService = googleReCaptchaService;
        }

        public IActionResult Index() => View(new ContactUs());

        public IActionResult About() => View();

        public IActionResult Contact() => View(new ContactUs());

        public IActionResult TermsAndConditions() => View();

        public IActionResult Privacy() => View();

        public IActionResult CookiePolicy() => View();

        public async Task<IActionResult> SendContactFormAsync(ContactUs model, InvokedFrom invokedFrom)
        {
            // recaptcha
            var riskScore = _googleReCaptchaService.CreateAssessment(model.Token, "SendContactForm");
            if (riskScore >= 0.7m)
            {
                Validate(model);
                if (ModelState.IsValid)
                {
                    await this._emailService.SendContactFormToEmail(model);

                    TempData.Add("ContactFormSent", "We have received your enquery and we will come back to you shortly.");
                }
            }

            switch (invokedFrom)
            {
                case InvokedFrom.HomePage:
                    return RedirectToAction(nameof(Index));
                case InvokedFrom.ContactUsPage:
                    return RedirectToAction(nameof(Contact));
            };
            return RedirectToAction(nameof(Index));
        }

        private void Validate(ContactUs model)
        {
            if (string.IsNullOrEmpty(model.FirstName) || (model.FirstName.Length < MIN_NAME_LENGTH || model.FirstName.Length > MAX_NAME_LENGTH))
            {
                ModelState.AddModelError(nameof(ContactUs.FirstName), $"First name cannot be less than {MIN_NAME_LENGTH} and more than {MAX_NAME_LENGTH} symbols");
            }
            if (string.IsNullOrEmpty(model.LastName) || (model.LastName.Length < MIN_NAME_LENGTH || model.LastName.Length > MAX_NAME_LENGTH))
            {
                ModelState.AddModelError(nameof(ContactUs.LastName), $"Last name cannot be less than {MIN_NAME_LENGTH} and more than {MAX_NAME_LENGTH} symbols");
            }
            if (string.IsNullOrEmpty(model.Email) || (ValidationHelper.RegexValidation(model.Email, EMAIL_REGEX) is false))
            {
                ModelState.AddModelError(nameof(ContactUs.Email), $"Invalid Email");
            }
            if (string.IsNullOrEmpty(model.Subject))
            {
                ModelState.AddModelError(nameof(ContactUs.Subject), $"Subject cannot be empty");
            }
            if (string.IsNullOrEmpty(model.EmailContent))
            {
                ModelState.AddModelError(nameof(ContactUs.EmailContent), $"Content cannot be empty");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
        }
    }
}

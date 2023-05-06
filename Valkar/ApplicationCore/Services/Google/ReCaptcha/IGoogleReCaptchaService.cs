namespace ApplicationCore.Services.Google.ReCaptcha
{
    public interface IGoogleReCaptchaService
    {
        /// <summary>
        /// Create assessment and return risk assessment analysis
        /// <para>IMPORTANT: If risk analysis score is less than 0.9 there is a risk of spam or bot usage</para>
        /// </summary>
        /// <param name="token">client side generated token</param>
        decimal CreateAssessment(string token, string recaptchaAction);
    }
}

namespace ApplicationCore.Services.Google.ReCaptcha
{
    using ApplicationCore.Config;
    using global::Google.Api.Gax.ResourceNames;
    using global::Google.Apis.Auth.OAuth2;
    using global::Google.Cloud.RecaptchaEnterprise.V1;
    using Grpc.Auth;
    using Microsoft.Extensions.Options;

    public class GoogleReCaptchaService : IGoogleReCaptchaService
    {
        private readonly IOptions<ApplicationCoreOptions> _options;

        public GoogleReCaptchaService(IOptions<ApplicationCoreOptions> options)
            => this._options = options;

        public decimal CreateAssessment(string token, string recaptchaAction)
        {
            var credential = GoogleCredential.FromFile("Config\\valkar-service-account.json")
                    .CreateScoped(RecaptchaEnterpriseServiceClient.DefaultScopes);

            // Create a new instance of the RecaptchaEnterpriseServiceClient.
            var client = new RecaptchaEnterpriseServiceClientBuilder
            {
                ChannelCredentials = credential.ToChannelCredentials()
            }.Build();

            // Build the assessment request.
            var createAssessmentRequest = new CreateAssessmentRequest()
            {
                Assessment = new Assessment()
                {
                    // Set the properties of the event to be tracked.
                    Event = new Event()
                    {
                        SiteKey = _options.Value.GoogleCaptchaKeyId,
                        Token = token,
                        ExpectedAction = recaptchaAction
                    },
                },
                ParentAsProjectName = new ProjectName("valkar")
            };

            var response = client.CreateAssessment(createAssessmentRequest);

            // TODO: log response.RiskAnalysis.Reasons

            return (decimal)response.RiskAnalysis.Score;
        }
    }
}

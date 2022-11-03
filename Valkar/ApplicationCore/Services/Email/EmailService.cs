namespace ApplicationCore.Services.Email
{
    using System.Threading.Tasks;
    using MimeKit;
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using ApplicationCore.ServiceModels.ContactUsForm;
    using MimeKit.Text;

    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(EmailConfiguration emailConfig)
            => this._emailConfig = emailConfig;

        public async Task SendEmail(Message message)
        {
            // Create email message
            var emailMessage = CreateEmailMessage(message);

            // Send the composed email
            await SendAsync(emailMessage);
        }

        public async Task SendContactFormToEmail(ContactUs contactUsForm)
        {
            // Convert to MIME message
            var emailMessage = new MimeMessage();
            
            // From - service email
            emailMessage.From.Add(
                new MailboxAddress(this._emailConfig.From));

            // To - help desk email
            emailMessage.To.Add(
                new MailboxAddress(this._emailConfig.HelpDeskEmail));

            // Subject - include First and last name follwed by subject
            emailMessage.Subject = $"{contactUsForm.Subject}";

            // Body
            var bodyBuilder = new BodyBuilder()
            {
                TextBody = string.Empty,
                HtmlBody = $@"<p><strong>Name:</strong> {contactUsForm.FirstName} {contactUsForm.LastName}</p>
                            <p><strong>Email:</strong> {contactUsForm.Email}</p>
                            <p><strong>Message:</strong> {contactUsForm.EmailContent}</p>"
            };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            // Send email async
            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            // From
            emailMessage.From.Add(
                new MailboxAddress(this._emailConfig.From));
            // To
            emailMessage.To.AddRange(message.To);
            // Subject
            emailMessage.Subject = message.Subject;
            // Body
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = message.Content
            };
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage emailMessage)
        {
            using var client = new SmtpClient();

            client.CheckCertificateRevocation = false;

            // Connect to the SmtpServer
            await client.ConnectAsync(
                this._emailConfig.SmtpServer,
                this._emailConfig.Port,
                SecureSocketOptions.StartTls);

            // Authenticate credentials for Gmail/Outlook account
            await client.AuthenticateAsync(this._emailConfig.UserName, this._emailConfig.Password);
            
            // Send the email
            client.Send(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}

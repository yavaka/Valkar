namespace ApplicationCore.Services.Email
{
    using System.Threading.Tasks;
    using MimeKit;
    using MailKit.Net.Smtp;
    using MailKit.Security;

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

            // Connect to the SmtpServer
            await client.ConnectAsync(
                this._emailConfig.SmtpServer,
                this._emailConfig.Port,
                SecureSocketOptions.StartTls);

            // Authenticate credentials for Gmail account
            await client.AuthenticateAsync(this._emailConfig.UserName, this._emailConfig.Password);

            // Send the email
            client.Send(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}

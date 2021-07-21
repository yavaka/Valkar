namespace ApplicationCore.Services.Email
{
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task SendEmail(Message message);
    }
}

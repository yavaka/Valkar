namespace ApplicationCore.Services.Email
{
    using ApplicationCore.ServiceModels.ContactUsForm;
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task SendEmail(Message message);

        Task SendContactFormToEmail(ContactUs contactUsForm);
    }
}

using System.Threading.Tasks;

namespace AspNetCoreIdentityUITest.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}

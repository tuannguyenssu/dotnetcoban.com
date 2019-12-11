using System.Threading.Tasks;

namespace AspNetCoreIdentityUITest.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}

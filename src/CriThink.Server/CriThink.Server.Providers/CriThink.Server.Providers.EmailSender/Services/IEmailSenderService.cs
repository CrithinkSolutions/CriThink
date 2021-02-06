using System.Threading.Tasks;

namespace CriThink.Server.Providers.EmailSender.Services
{
    public interface IEmailSenderService
    {
        Task SendAccountConfirmationEmailAsync(string recipient, string userId, string encodedCode, string userName);
        Task SendPasswordResetEmailAsync(string recipient, string userId, string encodedCode, string userName);
    }
}
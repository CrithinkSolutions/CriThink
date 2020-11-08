using System.Collections.Generic;
using System.Threading.Tasks;

namespace CriThink.Server.Providers.EmailSender.Providers
{
    public interface IEmailSenderProvider
    {
        Task Send(string fromAddress, IEnumerable<string> recipients, string subject, string htmlBody);
    }
}

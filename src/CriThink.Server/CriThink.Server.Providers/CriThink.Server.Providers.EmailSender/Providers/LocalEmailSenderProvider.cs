using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CriThink.Server.Providers.EmailSender.Providers
{
    public class LocalEmailSenderProvider : IEmailSenderProvider
    {
        public async Task Send(string fromAddress, IEnumerable<string> recipients, string subject, string htmlBody)
        {
            var smtpClient = new SmtpClient
            {
                Host = "localhost",
                Port = 2525,
            };

            using var message = new MailMessage(fromAddress, recipients.First())
            {
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true,
            };

            await smtpClient.SendMailAsync(message);
        }
    }
}

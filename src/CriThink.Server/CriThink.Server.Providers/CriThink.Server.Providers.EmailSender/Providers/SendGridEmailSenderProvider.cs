using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CriThink.Server.Providers.EmailSender.Providers
{
    public class SendGridEmailSenderProvider : IEmailSenderProvider
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly ILogger<SendGridEmailSenderProvider> _logger;

        public SendGridEmailSenderProvider(
            ISendGridClient sendGridClient,
            ILogger<SendGridEmailSenderProvider> logger)
        {
            _sendGridClient = sendGridClient ??
                throw new ArgumentNullException(nameof(sendGridClient));

            _logger = logger;
        }

        public async Task SendAsync(
            string fromAddress,
            IEnumerable<string> recipients,
            string subject,
            string htmlBody)
        {
            if (recipients is null)
                throw new ArgumentNullException(nameof(recipients));

            var sendGridMessage = new SendGridMessage
            {
                From = new EmailAddress(fromAddress),
            };

            foreach (var recipient in recipients)
            {
                sendGridMessage.AddTo(recipient);
            }

            sendGridMessage.SetSubject(subject);
            sendGridMessage.AddContent("text/html", htmlBody);

            var response = await _sendGridClient.SendEmailAsync(sendGridMessage);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.DeserializeResponseBodyAsync();

                _logger.LogError("Failed to send email using SendGrid. Status Code: {sc}; ResponseBody: {body}",
                    response.StatusCode,
                    body);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using CriThink.Server.Web.Settings;
using Microsoft.Extensions.Options;

namespace CriThink.Server.Web.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptionsSnapshot<AWSSESSettings> options)
        {
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public AWSSESSettings Options { get; }

        public Task SendEmailAsync(List<string> recipients, string subject, string message)
        {
            if (recipients == null)
                throw new ArgumentNullException(nameof(recipients));

            if (!recipients.Any())
                throw new InvalidOperationException("List of recipients is empty");

            return Execute(recipients, subject, message);
        }

        private Task Execute(IEnumerable<string> emails, string subject, string message)
        {
            var fromAddress = Options.FromAddress;

            using var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUCentral1);
            var sendRequest = new SendEmailRequest
            {
                Source = fromAddress,
                Destination = new Destination
                {
                    ToAddresses = new List<string>(emails)
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = message
                        }
                    }
                }
            };

            return client.SendEmailAsync(sendRequest);
        }
    }

    /// <summary>
    /// Contract to send emails
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Send an email to the given recipients with given data
        /// </summary>
        /// <param name="recipients">List of recipients</param>
        /// <param name="subject">Email subject</param>
        /// <param name="message">Email message body text</param>
        /// <returns></returns>
        Task SendEmailAsync(List<string> recipients, string subject, string message);
    }
}

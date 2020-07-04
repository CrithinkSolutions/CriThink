using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Server.Web.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CriThink.Server.Web.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IOptionsSnapshot<SendGridSettings> options, IConfiguration configuration)
        {
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public SendGridSettings Options { get; }

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
            var apiKey = _configuration["SendGridOptions-Key"];
            var fromAddress = Options.FromAddress;
            var fromName = Options.FromName;

            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress(fromAddress, fromName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            foreach (var email in emails)
            {
                msg.AddTo(new EmailAddress(email));
            }

            msg.TrackingSettings = new TrackingSettings
            {
                ClickTracking = new ClickTracking { Enable = false }
            };

            Task response = client.SendEmailAsync(msg);
            return response;
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

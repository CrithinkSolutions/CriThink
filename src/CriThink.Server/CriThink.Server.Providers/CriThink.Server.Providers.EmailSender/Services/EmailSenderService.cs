using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using CriThink.Server.Providers.EmailSender.Settings;
using CriThink.Server.RazorViews.Services;
using CriThink.Server.RazorViews.Views.Emails.ConfirmAccount;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CriThink.Server.Providers.EmailSender.Services
{
    internal class EmailSenderService : IEmailSenderService
    {
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EmailSettings _emailSettings;
        private readonly IWebHostEnvironment _environment;

        public EmailSenderService(IRazorViewToStringRenderer razorViewToStringRenderer, IHttpContextAccessor httpContextAccessor, IOptionsSnapshot<EmailSettings> emailSettings, IWebHostEnvironment environment)
        {
            _razorViewToStringRenderer = razorViewToStringRenderer ?? throw new ArgumentNullException(nameof(razorViewToStringRenderer));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _emailSettings = emailSettings?.Value ?? throw new ArgumentNullException(nameof(emailSettings));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public async Task SendAccountConfirmationEmailAsync(string recipient, string userId, string encodedCode)
        {
            var hostname = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var subject = _emailSettings.ConfirmationEmailSubject;

            var callbackUrl = string.Format(CultureInfo.InvariantCulture, _emailSettings.ConfirmationEmailLink, hostname, userId, encodedCode);
            var confirmAccountModel = new ConfirmAccountEmailViewModel(callbackUrl);

            var htmlBody = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/ConfirmAccount/ConfirmAccountEmail.cshtml", confirmAccountModel);

            await Execute(new[] { recipient }, subject, htmlBody).ConfigureAwait(false);
        }

        public async Task SendPasswordResetEmailAsync(string recipient, string userId, string encodedCode)
        {
            var hostname = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var subject = _emailSettings.ForgotPasswordSubject;

            var callbackUrl = string.Format(CultureInfo.InvariantCulture, _emailSettings.ForgotPasswordLink, hostname, userId, encodedCode);
            var confirmAccountModel = new ConfirmAccountEmailViewModel(callbackUrl);

            // TODO: custom email for this scope
            var htmlBody = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/ConfirmAccount/ConfirmAccountEmail.cshtml", confirmAccountModel);

            await Execute(new[] { recipient }, subject, htmlBody).ConfigureAwait(false);
        }

        private Task Execute(IEnumerable<string> recipients, string subject, string htmlBody)
        {
            var fromAddress = _emailSettings.FromAddress;

            return _environment.IsDevelopment()
                ? SendFromLocahost(fromAddress, recipients, subject, htmlBody)
                : SendFromAWS(fromAddress, recipients, subject, htmlBody);
        }
    
        private static Task SendFromLocahost(string fromAddress, IEnumerable<string> recipients, string subject, string htmlBody)
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

            return smtpClient.SendMailAsync(message);
        }

        private static Task SendFromAWS(string fromAddress, IEnumerable<string> recipients, string subject, string htmlBody)
        {
            using var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUCentral1);
            var sendRequest = new SendEmailRequest
            {
                Source = fromAddress,
                Destination = new Destination
                {
                    ToAddresses = new List<string>(recipients)
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = htmlBody
                        }
                    }
                }
            };

            return client.SendEmailAsync(sendRequest);
        }
    }
}
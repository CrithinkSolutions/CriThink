using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using CriThink.Server.Providers.EmailSender.Providers;
using CriThink.Server.Providers.EmailSender.Settings;
using CriThink.Server.RazorViews.Services;
using CriThink.Server.RazorViews.Views.Emails.ConfirmAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CriThink.Server.Providers.EmailSender.Services
{
    internal class EmailSenderService : IEmailSenderService
    {
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EmailSettings _emailSettings;
        private readonly IEmailSenderProvider _emailSenderProvider;

        public EmailSenderService(IRazorViewToStringRenderer razorViewToStringRenderer, IHttpContextAccessor httpContextAccessor, IOptionsSnapshot<EmailSettings> emailSettings, IEmailSenderProvider emailSenderProvider)
        {
            _razorViewToStringRenderer = razorViewToStringRenderer ?? throw new ArgumentNullException(nameof(razorViewToStringRenderer));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _emailSettings = emailSettings?.Value ?? throw new ArgumentNullException(nameof(emailSettings));
            _emailSenderProvider = emailSenderProvider ?? throw new ArgumentNullException(nameof(emailSenderProvider));
        }

        public async Task SendAccountConfirmationEmailAsync(string recipient, string userId, string encodedCode)
        {
            var hostname = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var subject = _emailSettings.ConfirmationEmailSubject;

            var callbackUrl = string.Format(CultureInfo.InvariantCulture, _emailSettings.ConfirmationEmailLink, hostname, userId, encodedCode);
            var confirmAccountModel = new ConfirmAccountEmailViewModel(callbackUrl, hostname);

            var htmlBody = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/ConfirmAccount/ConfirmAccountEmail.cshtml", confirmAccountModel);

            await Execute(new[] { recipient }, subject, htmlBody).ConfigureAwait(false);
        }

        public async Task SendPasswordResetEmailAsync(string recipient, string userId, string encodedCode)
        {
            var hostname = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var subject = _emailSettings.ForgotPasswordSubject;

            var callbackUrl = string.Format(CultureInfo.InvariantCulture, _emailSettings.ForgotPasswordLink, hostname, userId, encodedCode);
            var confirmAccountModel = new ConfirmAccountEmailViewModel(callbackUrl, hostname);

            // TODO: custom email for this scope
            var htmlBody = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/ConfirmAccount/ConfirmAccountEmail.cshtml", confirmAccountModel);

            await Execute(new[] { recipient }, subject, htmlBody).ConfigureAwait(false);
        }

        private Task Execute(IEnumerable<string> recipients, string subject, string htmlBody)
        {
            var fromAddress = _emailSettings.FromAddress;

            return _emailSenderProvider.Send(fromAddress, recipients, subject, htmlBody);
        }
    }
}
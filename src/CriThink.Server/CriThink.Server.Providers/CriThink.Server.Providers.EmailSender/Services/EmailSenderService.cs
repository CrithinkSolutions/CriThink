using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using CriThink.Server.Providers.EmailSender.Exceptions;
using CriThink.Server.Providers.EmailSender.Providers;
using CriThink.Server.Providers.EmailSender.Settings;
using CriThink.Server.RazorViews.Services;
using CriThink.Server.RazorViews.Views.Emails.AccountDeletion;
using CriThink.Server.RazorViews.Views.Emails.AlertNotification;
using CriThink.Server.RazorViews.Views.Emails.ConfirmAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CriThink.Server.Providers.EmailSender.Services
{
    internal class EmailSenderService : IEmailSenderService
    {
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EmailSettings _emailSettings;
        private readonly IEmailSenderProvider _emailSenderProvider;
        private readonly ILogger<EmailSenderService> _logger;

        public EmailSenderService(
            IRazorViewToStringRenderer razorViewToStringRenderer,
            IHttpContextAccessor httpContextAccessor,
            IOptionsSnapshot<EmailSettings> emailSettings,
            IEmailSenderProvider emailSenderProvider,
            ILogger<EmailSenderService> logger)
        {
            _razorViewToStringRenderer = razorViewToStringRenderer ??
                throw new ArgumentNullException(nameof(razorViewToStringRenderer));

            _httpContextAccessor = httpContextAccessor ??
                throw new ArgumentNullException(nameof(httpContextAccessor));

            _emailSettings = emailSettings?.Value ??
                throw new ArgumentNullException(nameof(emailSettings));

            _emailSenderProvider = emailSenderProvider ??
                throw new ArgumentNullException(nameof(emailSenderProvider));

            _logger = logger;
        }

        public async Task SendAccountConfirmationEmailAsync(string recipient, string userId, string encodedCode, string userName)
        {
            var hostname = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var subject = _emailSettings.ConfirmationEmailSubject;

            var callbackUrl = string.Format(CultureInfo.InvariantCulture, _emailSettings.ConfirmationEmailLink, hostname, userId, encodedCode);
            var confirmAccountModel = new ConfirmAccountEmailViewModel(callbackUrl, hostname, userName);

            // TODO: Use nameof() for path composition
            var htmlBody = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/ConfirmAccount/ConfirmAccountEmail.cshtml", confirmAccountModel);

            await ExecuteAsync(new[] { recipient }, subject, htmlBody).ConfigureAwait(false);
        }

        public async Task SendPasswordResetEmailAsync(string recipient, string userId, string encodedCode, string userName)
        {
            var hostname = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var subject = _emailSettings.ForgotPasswordSubject;

            var callbackUrl = string.Format(CultureInfo.InvariantCulture, _emailSettings.ForgotPasswordLink, hostname, userId, encodedCode);
            var confirmAccountModel = new ConfirmAccountEmailViewModel(callbackUrl, hostname, userName);

            // TODO: custom email for this scope
            var htmlBody = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/ForgotPassword/ForgotPasswordEmail.cshtml", confirmAccountModel);

            await ExecuteAsync(new[] { recipient }, subject, htmlBody).ConfigureAwait(false);
        }

        public async Task SendUnknownDomainAlertEmailAsync(string unknownDomainUrl, string resquetedByEmail)
        {
            var unknownDomainAlertViewModel = new UnknownDomainAdminAlertViewModel(unknownDomainUrl, resquetedByEmail);

            // TODO: Use nameof() for path composition
            var htmlBody = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/UnknownDomainAlert/UnknownDomainAlertAdminEmail.cshtml", unknownDomainAlertViewModel);

            const string subject = "[ALERT] - New unknown domain received";

            await ExecuteAsync(new[] { _emailSettings.AdminEmailAddress }, subject, htmlBody);
        }

        public async Task SendIdentifiedNewsSourceEmailAsync(string recipient, string identifiedDomainUrl, string classification)
        {
            var unknownDomainAlertViewModel = new UnknownDomainAdminUserAlertViewModel(identifiedDomainUrl, classification);

            // TODO: Use nameof() for path composition
            var htmlBody = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/UnknownDomainAlert/UnknownDomainAlertUserEmail.cshtml", unknownDomainAlertViewModel);

            var subject = $"[{classification}] - We identified the domain";

            await ExecuteAsync(new[] { recipient }, subject, htmlBody);
        }

        public async Task SendAccountDeletionConfirmationEmailAsync(string recipient, string username)
        {
            var viewModel = new AccountDeletionViewModel(username);

            // TODO: Use nameof() for path composition
            var htmlBody = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/AccountDeletion/AccountDeletion.cshtml", viewModel);

            var subject = $"We have permanently deleted your account";

            await ExecuteAsync(new[] { recipient }, subject, htmlBody);
        }

        private async Task ExecuteAsync(IEnumerable<string> recipients, string subject, string htmlBody)
        {
            var fromAddress = _emailSettings.FromAddress;

            try
            {
                await _emailSenderProvider.SendAsync(
                    fromAddress,
                    recipients,
                    subject,
                    htmlBody);
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, "Error sending an email", subject);

                throw new CriThinkEmailSendingFailureException(
                    ex,
                    fromAddress,
                    recipients,
                    subject,
                    htmlBody);
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using CriThink.Server.Providers.EmailSender.Providers;
using CriThink.Server.Providers.EmailSender.Settings;
using CriThink.Server.RazorViews.Services;
using CriThink.Server.RazorViews.Views.Emails.AlertNotification;
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

        public async Task SendAccountConfirmationEmailAsync(string recipient, string userId, string encodedCode, string userName)
        {
            var hostname = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var subject = _emailSettings.ConfirmationEmailSubject;

            var callbackUrl = string.Format(CultureInfo.InvariantCulture, _emailSettings.ConfirmationEmailLink, hostname, userId, encodedCode);
            var confirmAccountModel = new ConfirmAccountEmailViewModel(callbackUrl, hostname, userName);

            // TODO: Use nameof() for path composition
            var htmlBody = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/ConfirmAccount/ConfirmAccountEmail.cshtml", confirmAccountModel);

            await Execute(new[] { recipient }, subject, htmlBody).ConfigureAwait(false);
        }

        public async Task SendPasswordResetEmailAsync(string recipient, string userId, string encodedCode, string userName)
        {
            var hostname = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var subject = _emailSettings.ForgotPasswordSubject;

            var callbackUrl = string.Format(CultureInfo.InvariantCulture, _emailSettings.ForgotPasswordLink, hostname, userId, encodedCode);
            var confirmAccountModel = new ConfirmAccountEmailViewModel(callbackUrl, hostname, userName);

            // TODO: custom email for this scope
            // TODO: Use nameof() for path composition
            var htmlBody = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/ConfirmAccount/ConfirmAccountEmail.cshtml", confirmAccountModel);

            await Execute(new[] { recipient }, subject, htmlBody).ConfigureAwait(false);
        }

        public async Task SendUnknownDomainAlertEmailAsync(string unknownDomainUrl)
        {
            var unknownDomainAlertViewModel = new UnknownDomainAlertViewModel(unknownDomainUrl);

            // TODO: Use real template
            // TODO: Use nameof() for path composition
            var htmlBody = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/UnknownDomainAlert/UnknownDomainAlertAdminEmail.cshtml", unknownDomainAlertViewModel);

            var subject = $"[ALERT] - New unknown domain received";

            await Execute(new[] { _emailSettings.AdminEmailAddress }, subject, htmlBody);
        }

        public async Task SendIdentifiedNewsSourceEmailAsync(string userEmail, string identifiedDomainUrl, string classification)
        {
            var unknownDomainAlertViewModel = new UnknownDomainAlertViewModel(identifiedDomainUrl);

            // TODO: Use real template
            // TODO: Use nameof() for path composition
            var htmlBody = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/UnknownDomainAlert/UnknownDomainAlertAdminEmail.cshtml", unknownDomainAlertViewModel);

            var subject = $"[{classification}] - We identified the domain";

            await Execute(new[] { userEmail }, subject, htmlBody);
        }

        private Task Execute(IEnumerable<string> recipients, string subject, string htmlBody)
        {
            var fromAddress = _emailSettings.FromAddress;

            return _emailSenderProvider.Send(fromAddress, recipients, subject, htmlBody);
        }
    }
}
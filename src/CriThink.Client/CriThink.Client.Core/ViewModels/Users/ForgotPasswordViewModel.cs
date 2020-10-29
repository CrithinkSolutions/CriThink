using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class ForgotPasswordViewModel : MvxViewModel
    {
        private readonly IIdentityService _identityService;

        public ForgotPasswordViewModel(IIdentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        private string _emailOrUsername;

        public string EmailOrUsername
        {
            get => _emailOrUsername;
            set => SetProperty(ref _emailOrUsername, value);
        }

        private IMvxAsyncCommand _sendRequestCommand;

        public IMvxAsyncCommand SendRequestCommand => _sendRequestCommand ??= _sendRequestCommand = new MvxAsyncCommand(DoSendRequestCommand);

        private async Task DoSendRequestCommand(CancellationToken cancellationToken)
        {
            var request = new ForgotPasswordRequest();

            var isEmail = Regex.IsMatch(EmailOrUsername, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

            if (isEmail)
                request.Email = EmailOrUsername.ToUpperInvariant();
            else
                request.UserName = EmailOrUsername.ToUpperInvariant();

            await _identityService.RequestTemporaryTokenAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}

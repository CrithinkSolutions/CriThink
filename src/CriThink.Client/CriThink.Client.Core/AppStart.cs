using System;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Models.Identity;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Xamarin.Essentials;

namespace CriThink.Client.Core
{
    public class AppStart : MvxAppStart, IDisposable
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationService _applicationService;
        private readonly IMvxLog _logger;

        private bool _isDisposed;

        public AppStart(IMvxApplication application, IMvxNavigationService navigationService, IIdentityService identityService, IApplicationService applicationService, IMvxLog logger)
            : base(application, navigationService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
            _logger = logger;

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
            try
            {
                var isFirstStart = _applicationService.IsFirstStart();
                if (isFirstStart)
                {
                    await PerformFirstNavigationAsync().ConfigureAwait(true);
                    return;
                }

                var loggedUser = await _identityService.GetLoggedUserAsync().ConfigureAwait(false);
                if (loggedUser == null)
                {
                    await NavigateToSignUpViewAsync().ConfigureAwait(true);
                }
                else
                {
                    await PerformLoginAndNavigationToHomeAsync(loggedUser).ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                _logger?.Log(MvxLogLevel.Fatal, () => "Error initializing app start", ex, hint);
                throw;
            }
        }

        private async Task PerformFirstNavigationAsync()
        {
            await _applicationService.SetFirstAppStartAsync().ConfigureAwait(false);
            await NavigationService.Navigate<WelcomeViewModel>().ConfigureAwait(true);
        }

        private Task NavigateToSignUpViewAsync() => NavigationService.Navigate<SignUpViewModel>();

        private async Task PerformLoginAndNavigationToHomeAsync(User loggedUser)
        {
            using var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(45));

            var request = new UserLoginRequest
            {
                UserName = loggedUser.UserName,
                Email = loggedUser.UserEmail,
                Password = loggedUser.Password
            };

            try
            {
                await _identityService.PerformLoginAsync(request, cancellationToken.Token)
                    .ConfigureAwait(false);

                await NavigationService.Navigate<HomeViewModel>(cancellationToken: cancellationToken.Token)
                    .ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                _logger?.Log(MvxLogLevel.Error, () => "Error performing auto login", ex);
                await NavigateToLoginViewAsync(cancellationToken.Token).ConfigureAwait(true);
            }
        }

        private Task NavigateToLoginViewAsync(CancellationToken cancellationToken) => NavigationService.Navigate<LoginViewModel>(cancellationToken: cancellationToken);

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.None ||
                e.NetworkAccess == NetworkAccess.Unknown ||
                e.NetworkAccess == NetworkAccess.Local)
            {
                var userDialogs = Mvx.IoCProvider.Resolve<IUserDialogs>();
                userDialogs.Toast("No internet connection", TimeSpan.FromSeconds(5));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            }

            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

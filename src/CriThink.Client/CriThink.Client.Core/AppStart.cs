﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.Users;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Xamarin.Essentials;

namespace CriThink.Client.Core
{
    public class AppStart : MvxAppStart, IDisposable
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationService _applicationService;
        private readonly ILogger<AppStart> _logger;

        private bool _isDisposed;

        public AppStart(IMvxApplication application,
            IMvxNavigationService navigationService,
            IIdentityService identityService,
            IApplicationService applicationService,
            ILogger<AppStart> logger)
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

                var loggedUser = await _identityService.GetLoggedUserAccessAsync().ConfigureAwait(false);
                if (loggedUser?.JwtToken is null ||
                    loggedUser.RefreshToken is null)
                {
                    await NavigateToSignUpViewAsync().ConfigureAwait(true);
                }
                else
                {
                    await NavigateToHomeAsync().ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, "Error initializing app start", hint);
                throw;
            }
            finally
            {
                _applicationService.IncrementAppStartCounter();
            }
        }

        private async Task PerformFirstNavigationAsync()
        {
            _logger?.LogInformation("First app navigation");
            await NavigationService.Navigate<WelcomeViewModel>().ConfigureAwait(true);
        }

        private Task NavigateToSignUpViewAsync()
        {
            _logger?.LogInformation($"User not logged. Navigating to {nameof(SignUpViewModel)}");
            return NavigationService.Navigate<SignUpViewModel>();
        }

        private async Task NavigateToHomeAsync()
        {
            _logger?.LogInformation($"User logged. Navigating to {nameof(HomeViewModel)}");

            using var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            try
            {
                await NavigationService.Navigate<HomeViewModel>(cancellationToken: cancellationToken.Token)
                    .ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, $"Error performing login at startup. Navigating to {nameof(LoginViewModel)}");
                _identityService.PerformLogout();
                await NavigateToLoginViewAsync(cancellationToken.Token).ConfigureAwait(true);
            }
        }

        private Task NavigateToLoginViewAsync(CancellationToken cancellationToken) =>
            NavigationService.Navigate<LoginViewModel>(cancellationToken: cancellationToken);

        private static void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
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

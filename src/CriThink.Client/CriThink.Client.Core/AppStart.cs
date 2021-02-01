using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Models.Identity;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core
{
    public class AppStart : MvxAppStart
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationService _applicationService;
        private readonly IMvxLog _logger;

        public AppStart(IMvxApplication application, IMvxNavigationService navigationService, IIdentityService identityService, IApplicationService applicationService, IMvxLogProvider logProvider)
            : base(application, navigationService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
            _logger = logProvider?.GetLogFor<AppStart>();
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
                if (loggedUser is null)
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
            _logger?.Info("First app navigation");

            await _applicationService.SetFirstAppStartAsync().ConfigureAwait(false);
            await NavigationService.Navigate<WelcomeViewModel>().ConfigureAwait(true);
        }

        private Task NavigateToSignUpViewAsync()
        {
            _logger?.Info($"User not logged. Navigating to {nameof(SignUpViewModel)}");
            return NavigationService.Navigate<SignUpViewModel>();
        }

        private async Task PerformLoginAndNavigationToHomeAsync(User loggedUser)
        {
            _logger?.Info($"User logged. Navigating to {nameof(HomeViewModel)}");

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
                _logger.FatalException($"Error performing login at startup. Navigating to {nameof(LoginViewModel)}", ex, loggedUser.UserName);
                await NavigateToLoginViewAsync(cancellationToken.Token).ConfigureAwait(true);
            }
        }

        private Task NavigateToLoginViewAsync(CancellationToken cancellationToken) =>
            NavigationService.Navigate<LoginViewModel>(cancellationToken: cancellationToken);
    }
}

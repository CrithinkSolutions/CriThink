using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.Users;
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

        public AppStart(IMvxApplication application, IMvxNavigationService navigationService, IIdentityService identityService, IApplicationService applicationService, IMvxLog logger)
            : base(application, navigationService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
            _logger = logger;
        }

        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
            try
            {
                var isFirstStart = _applicationService.IsFirstStart();
                if (isFirstStart)
                {
                    await _applicationService.SetFirstAppStartAsync().ConfigureAwait(false);
                    await NavigationService.Navigate<WelcomeViewModel>().ConfigureAwait(false);
                    return;
                }

                var loggedUser = await _identityService.GetLoggedUserAsync().ConfigureAwait(false);
                if (loggedUser == null)
                {
                    await NavigationService.Navigate<SignUpViewModel>().ConfigureAwait(true);
                }
                else
                {
                    await NavigationService.Navigate<HomeViewModel>().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger?.Log(MvxLogLevel.Fatal, () => "Error initializing app start", ex, hint);
                throw;
            }
        }
    }
}

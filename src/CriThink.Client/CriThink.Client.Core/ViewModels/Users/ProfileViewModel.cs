using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IIdentityService _identityService;
        private readonly IMvxLog _log;

        public ProfileViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IIdentityService identityService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _log = logProvider?.GetLogFor<ProfileViewModel>();

            ProfileImageTransformations = new List<ITransformation>
            {
                new CircleTransformation()
            };
        }

        #region Properties

        // TODO: real pic
        public string ProfileImagePath => "res:ic_text_logo";

        public List<ITransformation> ProfileImageTransformations { get; }

        private string _headerText;
        public string HeaderText
        {
            get => _headerText;
            set => SetProperty(ref _headerText, value);
        }

        private IMvxAsyncCommand _navigateToEditCommand;
        public IMvxAsyncCommand NavigateToEditCommand => _navigateToEditCommand ??= new MvxAsyncCommand(DoNavigateToEditCommand);

        #endregion

        public override void Prepare()
        {
            base.Prepare();
            _log?.Info("User navigates to profile view");
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            var user = await _identityService.GetLoggedUserAsync().ConfigureAwait(false);
            if (user != null)
            {
                HeaderText = string.Format(CultureInfo.CurrentCulture, LocalizedTextSource.GetText("Hello"), user.UserName);
            }
        }

        private async Task DoNavigateToEditCommand(CancellationToken cancellationToken)
        {
            //await _navigationService.Navigate<EditProfileViewModel>(cancellationToken: cancellationToken).ConfigureAwait(true);
        }
    }
}

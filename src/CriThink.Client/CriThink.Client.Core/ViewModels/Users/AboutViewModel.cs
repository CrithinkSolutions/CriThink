using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Models.Menu;
using CriThink.Client.Core.Services;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Xamarin.Essentials;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class AboutViewModel : BaseBottomViewViewModel
    {
        private readonly IIdentityService _identityService;
        private readonly IMvxLog _log;

        public AboutViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IIdentityService identityService)
            : base(logProvider, navigationService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _log = LogProvider?.GetLogFor<AboutViewModel>();

            ProfileImageTransformations = new List<ITransformation>
            {
                new CircleTransformation()
            };

            MenuCollection = new MvxObservableCollection<BaseMenuItem>
            {
                new HeaderModel(LocalizedTextSource.GetText("AccountSettings")),
                new MenuModel(LocalizedTextSource.GetText("PersonalInfo")),
                new MenuModel(LocalizedTextSource.GetText("Notifications")),

                new HeaderModel(LocalizedTextSource.GetText("Referrals")),
                new MenuModel(LocalizedTextSource.GetText("InviteFriends")),
                new HeaderModel(LocalizedTextSource.GetText("Support")),
                new MenuModel(LocalizedTextSource.GetText("HowWorks")),
                new MenuModel(LocalizedTextSource.GetText("GetHelp")),
                new MenuModel(LocalizedTextSource.GetText("GiveFeedback")),

                new HeaderModel(LocalizedTextSource.GetText("Legal")),
                new MenuModel(LocalizedTextSource.GetText("ToS")),
                new MenuModel(LocalizedTextSource.GetText("PrivacySettings")),

                new ActionModel(LocalizedTextSource.GetText("Logout"))
            };

            TabId = "profile";
        }

        #region Properties

        // TODO: real pic
        public string ProfileImagePath => "res:ic_text_logo";

        public MvxObservableCollection<BaseMenuItem> MenuCollection { get; }

        public List<ITransformation> ProfileImageTransformations { get; }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        #endregion

        #region Commands

        private IMvxAsyncCommand _navigateToProfileCommand;
        public IMvxAsyncCommand NavigateToProfileCommand => _navigateToProfileCommand ??= new MvxAsyncCommand(DoNavigateToProfile);

        #endregion

        public override void Prepare()
        {
            base.Prepare();
            _log?.Info("User navigates to about view");
            VersionTracking.Track();
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            var version = string.Format(CultureInfo.CurrentCulture, LocalizedTextSource.GetText("Version"), VersionTracking.CurrentVersion);
            MenuCollection.Add(new VersionModel(version));

            var user = await _identityService.GetLoggedUserAsync().ConfigureAwait(false);
            if (user != null)
            {
                Username = user.UserName;
            }
        }

        private async Task DoNavigateToProfile(CancellationToken cancellationToken)
        {
            await NavigationService.Navigate<ProfileViewModel>(cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}

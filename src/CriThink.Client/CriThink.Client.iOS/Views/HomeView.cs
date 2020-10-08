using CriThink.Client.Core.ViewModels;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;

namespace CriThink.Client.iOS.Views
{
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class HomeView : MvxViewController<HomeViewModel>
    {
        public HomeView() /*: base("HomeView", null)*/
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = CreateBindingSet();

            set.Apply();
        }
    }
}
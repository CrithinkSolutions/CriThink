using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.NewsChecker;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.NewsChecker
{
    [MvxFragmentPresentation(typeof(HomeViewModel), null)]
    [Register(nameof(NewsCheckerView))]
    public class NewsCheckerView : MvxFragment<NewsCheckerViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.newschecker_view, null);

            var txtWelcome = view.FindViewById<AppCompatTextView>(Resource.Id.txtWelcome);
            var txtName = view.FindViewById<AppCompatTextView>(Resource.Id.txtName);
            var txtMotto = view.FindViewById<AppCompatTextView>(Resource.Id.txtMotto);
            var txtDate = view.FindViewById<AppCompatTextView>(Resource.Id.txtDate);
            var btnNews = view.FindViewById<AppCompatButton>(Resource.Id.btnNews);

            var set = CreateBindingSet();

            set.Bind(txtWelcome).To(vm => vm.WelcomeText);
            set.Bind(txtName).To(vm => vm.Username);
            set.Bind(txtMotto).ToLocalizationId("Motto");
            set.Bind(txtDate).To(vm => vm.TodayDate);
            set.Bind(btnNews).For(v => v.Text).ToLocalizationId("NewsLinkHint");
            set.Bind(btnNews).To(vm => vm.NavigateNewsCheckerCommand);

            set.Apply();

            return view;
        }
    }
}
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.NewsChecker;
using Google.Android.Material.TextField;
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

            var txtInputUri = view.FindViewById<TextInputEditText>(Resource.Id.txtInput_uri);
            var btnCheckUri = view.FindViewById<AppCompatButton>(Resource.Id.btn_CheckUri);

            var set = CreateBindingSet();

            set.Bind(txtInputUri).To(vm => vm.News);
            set.Bind(btnCheckUri).To(vm => vm.CheckNewsCommand);

            set.Apply();

            return view;
        }
    }
}
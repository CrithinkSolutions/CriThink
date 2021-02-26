using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Droid.Controls;
using CriThink.Client.Droid.Views.DebunkingNews;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.NewsChecker
{
    [MvxFragmentPresentation(typeof(HomeViewModel), "tabs_container_frame")]
    [Register(nameof(NewsCheckerView))]
    public class NewsCheckerView : MvxFragment<NewsCheckerViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.newschecker_view, null);

            var loader = view.FindViewById<LoaderView>(Resource.Id.loader);
            var txtWelcome = view.FindViewById<AppCompatTextView>(Resource.Id.txtWelcome);
            var txtName = view.FindViewById<AppCompatTextView>(Resource.Id.txtName);
            var txtMotto = view.FindViewById<AppCompatTextView>(Resource.Id.txtMotto);
            var txtDate = view.FindViewById<AppCompatTextView>(Resource.Id.txtDate);
            var btnNews = view.FindViewById<AppCompatButton>(Resource.Id.btnNews);
            var txtSeeAll = view.FindViewById<AppCompatTextView>(Resource.Id.txtSeeAll);
            var imgLogo = view.FindViewById<MvxCachedImageView>(Resource.Id.imgLogo);

            var txtSectionTitle = view.FindViewById<AppCompatTextView>(Resource.Id.txtSectionTitle);
            var listDebunkingNews = view.FindViewById<MvxRecyclerView>(Resource.Id.list_debunkingNews);

            var layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Horizontal, false);
            listDebunkingNews.SetLayoutManager(layoutManager);
            // Workaround to avoid a crash when navigating from login view model.
            // Seems to be a recycler view bug: https://stackoverflow.com/a/58540280/3415073
            listDebunkingNews.SetItemAnimator(null);

            var adapter = new DebunkingNewsHorizontalAdapter((IMvxAndroidBindingContext) BindingContext);
            listDebunkingNews.Adapter = adapter;

            var set = CreateBindingSet();

            set.Bind(txtWelcome).To(vm => vm.WelcomeText);
            set.Bind(txtName).To(vm => vm.Username);
            set.Bind(txtMotto).ToLocalizationId("Motto");
            set.Bind(txtDate).To(vm => vm.TodayDate);
            set.Bind(btnNews).For(v => v.Text).ToLocalizationId("NewsLinkHint");
            set.Bind(btnNews).To(vm => vm.NavigateNewsCheckerCommand);
            set.Bind(adapter).For(v => v.ItemsSource).To(vm => vm.Feed);
            set.Bind(listDebunkingNews).For(v => v.ItemClick).To(vm => vm.DebunkingNewsSelectedCommand);
            set.Bind(txtSeeAll).ToLocalizationId("SeeAll");
            set.Bind(txtSeeAll).For("Click").To(vm => vm.NavigateToAllDebunkingNewsCommand);
            set.Bind(imgLogo).For(v => v.Transformations).To(vm => vm.LogoImageTransformations);
            set.Bind(imgLogo).For(v => v.ImagePath).To(vm => vm.ProfileImagePath);

            set.Bind(loader).For(v => v.BindVisible()).To(vm => vm.IsLoading);

            set.Bind(txtSectionTitle).ToLocalizationId("DebunkingNewsTitle");

            set.Apply();

            return view;
        }
    }
}
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using Com.Airbnb.Lottie;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Droid.Controls;
using CriThink.Client.Droid.Views.Users;
using Google.Android.Material.TextField;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Plugin.Visibility;
using static Android.Views.View;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.NewsChecker
{
    [MvxActivityPresentation]
    [Activity]
    public class CheckNewsView : MvxActivity<CheckNewsViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.checknews_view);
            MainApplication.SetGradientStatusBar(this);
            var txtRecentSearch = FindViewById<AppCompatTextView>(Resource.Id.txtRecentSearch);
            var recyclerRecentSearch = FindViewById<MvxRecyclerView>(Resource.Id.recyclerRecentSearch);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            var txtTitle = FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            var txtInputSearch = FindViewById<TextInputLayout>(Resource.Id.txtInput_search);
            var txtEditSearch = FindViewById<BindableEditText>(Resource.Id.txtEdit_search);
            var txtNoSearches = FindViewById<AppCompatTextView>(Resource.Id.txtNoSearches);
            var animEmpty = FindViewById<LottieAnimationView>(Resource.Id.animEmpty);

            txtInputSearch.SetEndIconOnClickListener(new EditTextIconClickListener(this, ViewModel, PositionClickItem.End));
            txtInputSearch.SetStartIconOnClickListener(new EditTextIconClickListener(this, ViewModel, PositionClickItem.Start));
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);

            var layoutManager = new LinearLayoutManager(this) { AutoMeasureEnabled = true };
            recyclerRecentSearch.SetLayoutManager(layoutManager);
            recyclerRecentSearch.SetItemAnimator(null);
            recyclerRecentSearch.Adapter = new RecentNewsChecksAdapter(BindingContext as IMvxAndroidBindingContext);

            recyclerRecentSearch.ItemTemplateSelector = new RecentSearchSelector();

            var set = CreateBindingSet();

            set.Bind(txtRecentSearch).ToLocalizationId("RecentSearch");
            set.Bind(txtInputSearch).For(v => v.PlaceholderText).ToLocalizationId("NewsLinkHint");
            set.Bind(txtTitle).ToLocalizationId("Title");
            set.Bind(txtNoSearches).ToLocalizationId("NoSearches");
            set.Bind(txtEditSearch).To(vm => vm.SearchText);
            set.Bind(txtEditSearch).For(v => v.KeyCommand).To(vm => vm.SubmitUriCommand);
            set.Bind(recyclerRecentSearch).For(v => v.ItemsSource).To(vm => vm.RecentNewsChecksCollection);
            set.Bind(recyclerRecentSearch).For(v => v.ItemClick).To(vm => vm.RepeatSearchCommand);

            set.Bind(animEmpty).For(v => v.Visibility).To(vm => vm.IsFirstSearch).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(txtNoSearches).For(v => v.Visibility).To(vm => vm.IsFirstSearch).WithConversion<MvxVisibilityValueConverter>();

            set.Bind(txtRecentSearch).For(v => v.Visibility).To(vm => vm.RecentSearchesIsNotEmpty).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(recyclerRecentSearch).For(v => v.Visibility).To(vm => vm.RecentSearchesIsNotEmpty).WithConversion<MvxVisibilityValueConverter>();

            set.Apply();
        }

        public override bool OnSupportNavigateUp()
        {
            Finish();
            return false;
        }

        internal class EditTextIconClickListener : Java.Lang.Object, IOnClickListener
        {
            private PositionClickItem _positionClickItem;
            private CheckNewsViewModel _checkNewsViewModel;
            private Context _context;

            public async void OnClick(View v)
            {
                switch (_positionClickItem)
                {
                    case PositionClickItem.Start:
                        var clipboardManager = _context.GetSystemService(Context.ClipboardService) as ClipboardManager;
                        var text = clipboardManager.PrimaryClip?.GetItemAt(0)?.Text?.ToString();
                        _checkNewsViewModel.SearchText = text;
                        break;
                    case PositionClickItem.End:
                        await _checkNewsViewModel.SubmitUriCommand.ExecuteAsync();
                        break;
                }
            }

            internal EditTextIconClickListener(
                Context context,
                CheckNewsViewModel checkNewsViewModel,
                PositionClickItem positionClickItem
                )
            {
                _positionClickItem = positionClickItem;
                _checkNewsViewModel = checkNewsViewModel;
                _context = context;
            }


        }

    }
    enum PositionClickItem
    {
        Start,
        End
    }
}
using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Droid.Controls;
using Google.Android.Material.BottomSheet;
using Google.Android.Material.TextField;
using Java.Interop;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.Material;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Views.Fragments;
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
            var txtRecentSearch = FindViewById<AppCompatTextView>(Resource.Id.txtRecentSearch);
            var recyclerRecentSearch = FindViewById<MvxRecyclerView>(Resource.Id.recyclerRecentSearch);
            var toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            var txtTitle = FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            var txtInputSearch = FindViewById<TextInputLayout>(Resource.Id.txtInput_search);
            var txtEditSearch = FindViewById<BindableEditText>(Resource.Id.txtEdit_search);

            txtInputSearch.SetEndIconOnClickListener(new EditTextIconClickListener(this, ViewModel, PositionClickItem.End));
            txtInputSearch.SetStartIconOnClickListener(new EditTextIconClickListener(this, ViewModel, PositionClickItem.Start));
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);
            Window.SetStatusBarColor(new Color(ContextCompat.GetColor(this, Resource.Color.accent)));

            var layoutManager = new LinearLayoutManager(this);
            recyclerRecentSearch.SetLayoutManager(layoutManager);
            recyclerRecentSearch.SetItemAnimator(null);
            recyclerRecentSearch.Adapter = new RecentNewsChecksAdapter(BindingContext as IMvxAndroidBindingContext);
            var set = CreateBindingSet();
            set.Bind(txtRecentSearch).ToLocalizationId("RecentSearch");
            set.Bind(txtInputSearch).For(v => v.PlaceholderText).ToLocalizationId("NewsLinkHint");
            set.Bind(txtTitle).ToLocalizationId("Title");
            set.Bind(txtEditSearch).To(vm => vm.NewsUri);
            set.Bind(txtEditSearch).For(v => v.KeyCommand).To(vm => vm.SubmitUriCommand);
            set.Bind(recyclerRecentSearch).For(v => v.ItemsSource).To(vm => vm.RecentNewsChecksCollection);
            set.Bind(recyclerRecentSearch).For(v => v.ItemClick).To(vm => vm.RepeatSearchCommand);
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
                switch(_positionClickItem)
                {
                    case PositionClickItem.Start:
                        var clipboardManager = _context.GetSystemService(Context.ClipboardService) as ClipboardManager;
                        var text = clipboardManager.PrimaryClip?.GetItemAt(0)?.Text?.ToString();
                        _checkNewsViewModel.NewsUri = text;
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
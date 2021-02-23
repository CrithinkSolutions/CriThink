using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Droid.Controls;
using Google.Android.Material.BottomSheet;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.Material;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.NewsChecker
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(CheckNewsView))]
    public class CheckNewsView : MvxBottomSheetDialogFragment<CheckNewsViewModel>
    {
        private HomeView BaseActivity => (HomeView) Activity;

        public CheckNewsView()
        { }

        protected CheckNewsView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(StyleNormal, Resource.Style.AppBottomSheetDialogTheme);
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = base.OnCreateDialog(savedInstanceState);

            dialog.ShowEvent += (sender, args) =>
            {
                SetupFullHeight(dialog as BottomSheetDialog);
            };

            return dialog;
        }

        private void SetupFullHeight(BottomSheetDialog bottomSheetDialog)
        {
            int windowHeight = Resources.DisplayMetrics.HeightPixels;

            var bottomSheet = bottomSheetDialog.FindViewById<FrameLayout>(Resource.Id.design_bottom_sheet);
            ViewGroup.LayoutParams layoutParams = bottomSheet.LayoutParameters;
            layoutParams.Height = windowHeight;
            bottomSheet.LayoutParameters = layoutParams;

            bottomSheetDialog.Behavior.State = BottomSheetBehavior.StateExpanded;
            bottomSheetDialog.Behavior.PeekHeight = windowHeight;
            bottomSheetDialog.Behavior.SaveFlags = BottomSheetBehavior.SaveAll;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = View.Inflate(Context, Resource.Layout.checknews_view, null);

            var txtRecentSearch = view.FindViewById<AppCompatTextView>(Resource.Id.txtRecentSearch);
            var recyclerRecentSearch = view.FindViewById<MvxRecyclerView>(Resource.Id.recyclerRecentSearch);
            var toolbar = view.FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            var editTextSearch = view.FindViewById<BindableEditText>(Resource.Id.editTextSearch);
            var imgCancel = view.FindViewById<AppCompatImageView>(Resource.Id.imgCancel);

            HasOptionsMenu = true;
            BaseActivity.SetSupportActionBar(toolbar);
            BaseActivity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            BaseActivity.SupportActionBar.SetDisplayShowTitleEnabled(false);

            var layoutManager = new LinearLayoutManager(Activity);
            recyclerRecentSearch.SetLayoutManager(layoutManager);
            recyclerRecentSearch.Adapter = new RecentNewsChecksAdapter(BindingContext as IMvxAndroidBindingContext);

            var set = CreateBindingSet();

            set.Bind(txtRecentSearch).ToLocalizationId("RecentSearch");
            set.Bind(editTextSearch).For(v => v.Hint).ToLocalizationId("NewsLinkHint");
            set.Bind(editTextSearch).To(vm => vm.NewsUri);
            set.Bind(editTextSearch).For(v => v.KeyCommand).To(vm => vm.SubmitUriCommand);
            set.Bind(recyclerRecentSearch).For(v => v.ItemsSource).To(vm => vm.RecentNewsChecksCollection);
            set.Bind(recyclerRecentSearch).For(v => v.ItemClick).To(vm => vm.RepeatSearchCommand);
            set.Bind(imgCancel).For("Click").To(vm => vm.ClearTextCommand);

            set.Apply();

            return view;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Dismiss();
            return true;
        }
    }
}
using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.ViewModels.DebunkingNews;
using CriThink.Client.Droid.Extensions;
using CriThink.Client.Droid.Views.DebunkingNews.Adapters;
using Google.Android.Material.BottomSheet;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX;
using MvvmCross.DroidX.Material;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace CriThink.Client.Droid.Views.DebunkingNews.Dialogs
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(DebunkingNewsCollectionView))]
    public class DebunkingNewsCollectionView : MvxBottomSheetDialogFragment<DebunkingNewsCollectionViewModel>
    {
        public DebunkingNewsCollectionView()
        { }

        protected DebunkingNewsCollectionView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(StyleNormal, Resource.Style.AppBottomSheetDialogTheme);
        }

        public override void SetupDialog(Dialog dialog, int style)
        {
            base.SetupDialog(dialog, style);

            dialog?.Window?.ClearFlags(WindowManagerFlags.DimBehind);
            if (dialog is BottomSheetDialog bottomSheetDialog && Resources?.DisplayMetrics != null)
            {
                bottomSheetDialog.Behavior.PeekHeight = (int) (Resources?.DisplayMetrics?.HeightPixels * 0.55);
                bottomSheetDialog.Behavior.Hideable = false;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = View.Inflate(Context, Resource.Layout.debunkingnews_view, null);

            var txtSectionTitle = view?.FindViewById<AppCompatTextView>(Resource.Id.txtSectionTitle);
            var listDebunkingNews = view?.FindViewById<MvxRecyclerView>(Resource.Id.list_debunkingNews);
            var refresher = view?.FindViewById<MvxSwipeRefreshLayout>(Resource.Id.refresher);

            if (listDebunkingNews != null)
            {
                listDebunkingNews.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager(Activity);
                listDebunkingNews.SetLayoutManager(layoutManager);

                listDebunkingNews.Adapter = new DebunkingNewsAdapter(BindingContext as IMvxAndroidBindingContext);

                listDebunkingNews.AddOnScrollFetchItemsListener(
                    layoutManager,
                    () => ViewModel.FetchDebunkingNewsTask,
                    () => ViewModel.FetchDebunkingNewsCommand);
            }

            var set = CreateBindingSet();

            set.Bind(txtSectionTitle).ToLocalizationId("DebunkingNewsTitle");

            set.Bind(listDebunkingNews).For(v => v.ItemsSource).To(vm => vm.Feed);
            set.Bind(listDebunkingNews).For(v => v.ItemClick).To(vm => vm.DebunkingNewsSelectedCommand);

            set.Bind(refresher).For(v => v.Refreshing).To(vm => vm.IsRefreshing);
            set.Bind(refresher).For(v => v.RefreshCommand).To(vm => vm.RefreshDebunkingNewsCommand);

            set.Apply();

            return view;
        }
    }
}
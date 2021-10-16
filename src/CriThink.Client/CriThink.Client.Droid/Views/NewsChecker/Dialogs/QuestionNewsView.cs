using System;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Droid.Constants;
using CriThink.Client.Droid.Views.NewsChecker.Adapters;
using Google.Android.Material.BottomSheet;
using MvvmCross.DroidX.Material;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace CriThink.Client.Droid.Views.NewsChecker.Dialogs
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(QuestionNewsView))]
    public class QuestionNewsView : MvxBottomSheetDialogFragment<QuestionNewsViewModel>
    {
        public QuestionNewsView()
        {
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(StyleNoFrame, Resource.Style.AppBottomSheetDialogTheme);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = View.Inflate(Context, Resource.Layout.newsquestion_dialog_view, null);
            var listQuestionNews = view.FindViewById<MvxRecyclerView>(Resource.Id.recyclerQuestions);
            var layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
            var adapter = new QuestionNewsAdapter((IMvxAndroidBindingContext) BindingContext);
            listQuestionNews.SetLayoutManager(layoutManager);
            listQuestionNews.Adapter = adapter;

            using (var set = CreateBindingSet())
            {
                set.Bind(adapter).For(v => v.ItemsSource).To(vm => vm.Questions);
            }

            
            Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Transparent));

            var behavior = (Dialog as BottomSheetDialog).Behavior;
            behavior.State = BottomSheetBehavior.StateCollapsed;
            behavior.PeekHeight = BottomSheetConstants.PeekHeight;
            behavior.HalfExpandedRatio = 0.99f;
            behavior.GestureInsetBottomIgnored = false;
            behavior.FitToContents = true;
            behavior.ExpandedOffset = 0;
            behavior.Hideable = false;
            behavior.SkipCollapsed = false;
            behavior.AddBottomSheetCallback(new BottomSheetToolbarToggleCallback(this));
            
            return view;
        }
    }
    public class BottomSheetToolbarToggleCallback : BottomSheetBehavior.BottomSheetCallback
    {
        public BottomSheetToolbarToggleCallback(BottomSheetDialogFragment bottomSheetDialogFragment)
        {
            this._bottomSheetDialogFragment = bottomSheetDialogFragment ?? throw new System.ArgumentNullException(nameof(bottomSheetDialogFragment));
        }
        public override void OnSlide(View bottomSheet, float slideOffset)
        {
        }
        public override void OnStateChanged(View bottomSheet, int newState)
        {
            switch (newState)
            {
                case BottomSheetBehavior.StateCollapsed:
                    ShowToolbar(bottomSheet, ViewStates.Gone);
                    break;

                case BottomSheetBehavior.StateHalfExpanded:
                    ShowToolbar(bottomSheet, ViewStates.Gone);
                    break;
                case BottomSheetBehavior.StateExpanded:
                    ShowToolbar(bottomSheet, ViewStates.Visible);
                    break;
                case BottomSheetBehavior.StateHidden:
                    _bottomSheetDialogFragment.Dismiss();
                    break;
            }
        }
        private void ShowToolbar(View bottomSheet, ViewStates viewState)
        {
            var toolbar = bottomSheet.FindViewById<Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                toolbar.Visibility = viewState;
            }
        }
        private readonly BottomSheetDialogFragment _bottomSheetDialogFragment;
    }
}
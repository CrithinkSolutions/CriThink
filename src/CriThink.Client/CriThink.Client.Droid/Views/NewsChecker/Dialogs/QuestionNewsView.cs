using System;
using Android.App;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.CardView.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.Core.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Droid.Constants;
using CriThink.Client.Droid.Views.NewsChecker.Adapters;
using Google.Android.Material.AppBar;
using Google.Android.Material.BottomSheet;
using Google.Android.Material.Card;
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
            SetStyle(StyleNormal, Resource.Style.AppBottomSheetDialogTheme);
        }

        public override void OnStart()
        {
            base.OnStart();
            Dialog?.Window.ClearFlags(WindowManagerFlags.DimBehind);
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = base.OnCreateDialog(savedInstanceState);
            dialog.SetCanceledOnTouchOutside(false);
            return dialog;

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = View.Inflate(Context, Resource.Layout.newsquestion_dialog_view, null);
            var listQuestionNews = view.FindViewById<MvxRecyclerView>(Resource.Id.recyclerQuestions);
            var toolbar = Activity.FindViewById<MaterialToolbar>(Resource.Id.appBarLayout);
            var layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
            var adapter = new QuestionNewsAdapter((IMvxAndroidBindingContext) BindingContext);
            listQuestionNews.SetLayoutManager(layoutManager);
            listQuestionNews.Adapter = adapter;

            using (var set = CreateBindingSet())
            {
                set.Bind(adapter).For(v => v.ItemsSource).To(vm => vm.Questions);
            }

            
            return view;
        }
    }

}
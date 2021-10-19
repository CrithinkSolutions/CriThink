using System;
using Android.Animation;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.Core.Content;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.Statistics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Plugin.Visibility;
using MvvmCross.WeakSubscription;
using static Android.Animation.ValueAnimator;
using static AndroidHUD.Resource;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Statistics
{
    [MvxFragmentPresentation(typeof(HomeViewModel), "tabs_container_frame")]
    [Register(nameof(HomeStatisticsView))]
    public class HomeStatisticsView : MvxFragment<HomeStatisticsViewModel>
    {
        private AppCompatTextView _txtActiveUsers;
        private AppCompatTextView _txtTotalSearches;
        private AppCompatTextView _txtUserSearches;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.homestatistics_view, null);

            var txtActiveUsersHeader = view.FindViewById<AppCompatTextView>(Resource.Id.txtActiveUsersHeader);
            _txtActiveUsers = view.FindViewById<AppCompatTextView>(Resource.Id.txtActiveUsers);
            var txtTotalSearchesHeader = view.FindViewById<AppCompatTextView>(Resource.Id.txtTotalSearchesHeader);
            _txtTotalSearches = view.FindViewById<AppCompatTextView>(Resource.Id.txtTotalSearches);
            var txtUserSearchesHeader = view.FindViewById<AppCompatTextView>(Resource.Id.txtUserSearchesHeader);
            _txtUserSearches = view.FindViewById<AppCompatTextView>(Resource.Id.txtUserSearches);
            var viewContent = view.FindViewById<ConstraintLayout>(Resource.Id.viewContent);
            var viewError = view.FindViewById<ConstraintLayout>(Resource.Id.viewError);
            var txtError = view.FindViewById<AppCompatTextView>(Resource.Id.txtError);


            ViewModel.WeakSubscribe(() => ViewModel.PlatformUsers, (sender, e) => CountAnimation(_txtActiveUsers, ViewModel.PlatformUsers));
            ViewModel.WeakSubscribe(() => ViewModel.PlatformSearches, (sender, e) => CountAnimation(_txtTotalSearches, ViewModel.PlatformSearches));
            ViewModel.WeakSubscribe(() => ViewModel.UserSearches, (sender, e) => CountAnimation(_txtUserSearches, ViewModel.UserSearches));

            var set = CreateBindingSet();

            set.Bind(viewContent).For(v => v.Visibility).To(vm => vm.HasFailed).WithConversion<MvxInvertedVisibilityValueConverter>();
            set.Bind(viewError).For(v => v.BindVisible()).To(vm => vm.HasFailed);

            set.Bind(txtActiveUsersHeader).ToLocalizationId("ActiveUsers");
            set.Bind(txtTotalSearchesHeader).ToLocalizationId("TotalSearches");
            set.Bind(txtUserSearchesHeader).ToLocalizationId("UserSearches");
            set.Bind(txtError).ToLocalizationId("Error");

            set.Apply();



            return view;
        }
        public override void OnStart()
        {
            base.OnStart();
            CountAnimation(_txtActiveUsers, ViewModel.PlatformUsers);
            CountAnimation(_txtTotalSearches, ViewModel.PlatformSearches);
            CountAnimation(_txtUserSearches, ViewModel.UserSearches);
        }
        void CountAnimation(AppCompatTextView textView, long value)
        {
            var animator = new ValueAnimator();
            animator.SetObjectValues(0, (int)value); 
            animator.AddUpdateListener(new CountAnimatorUpdateListener(textView));
            animator.SetDuration(Math.Min(5000, Math.Max(value / 100, 1000)));
            animator.Start();
        }
    }
    public class CountAnimatorUpdateListener : Java.Lang.Object, IAnimatorUpdateListener
    {
        private readonly AppCompatTextView _textView;
        public CountAnimatorUpdateListener(AppCompatTextView textView)
        {
            _textView = textView;
        }

        public void OnAnimationUpdate(ValueAnimator animation)
        {
            _textView.Text = $"{(int)animation.AnimatedValue:000,000}";
        }
    }

}
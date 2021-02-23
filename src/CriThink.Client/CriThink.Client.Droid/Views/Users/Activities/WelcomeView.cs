using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.Content;
using AndroidX.ViewPager.Widget;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.Users;
using DK.Ostebaronen.Droid.ViewPagerIndicator;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Views.ViewPager;
using MvvmCross.ViewModels;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [MvxActivityPresentation]
    [Activity(Label = "CriThink.WelcomeView")]
    public class WelcomeView : MvxActivity<WelcomeViewModel>
    {
        private WelcomeViewFragmentAdapter _adapter;
        private ViewPager _pager;
        private IPageIndicator _indicator;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.welcome_view);

            BuildViewPager();
        }

        private void BuildViewPager()
        {
            var fragments = new List<MvxViewPagerFragmentInfo>
            {
                new MvxViewPagerFragmentInfo(nameof(WelcomeFragment), "", typeof(WelcomeFragment), new MvxViewModelInstanceRequest(typeof(WelcomeImageViewModel))),
                new MvxViewPagerFragmentInfo(nameof(WelcomeFragment), "", typeof(WelcomeFragment), new MvxViewModelInstanceRequest(typeof(WelcomeImageViewModel))),
                new MvxViewPagerFragmentInfo(nameof(WelcomeFragment), "", typeof(WelcomeFragment), new MvxViewModelInstanceRequest(typeof(WelcomeImageViewModel))),
                new MvxViewPagerFragmentInfo(nameof(WelcomeFragment), "", typeof(WelcomeFragment), new MvxViewModelInstanceRequest(typeof(WelcomeImageViewModel))),
                new MvxViewPagerFragmentInfo(nameof(WelcomeFragment), "", typeof(WelcomeFragment), new MvxViewModelInstanceRequest(typeof(WelcomeImageViewModel))),
                new MvxViewPagerFragmentInfo(nameof(WelcomeLoginSignInView), "", typeof(WelcomeLoginSignInView), new MvxViewModelInstanceRequest(ViewModel.WelcomeLoginSignInViewModel)),
            };

            _adapter = new WelcomeViewFragmentAdapter(this, SupportFragmentManager, fragments);
            _pager = FindViewById<ViewPager>(Resource.Id.pager);
            if (_pager != null)
                _pager.Adapter = _adapter;

            _indicator = FindViewById<CirclePageIndicator>(Resource.Id.indicator);
            if (_indicator is CirclePageIndicator circleIndicator)
            {
                circleIndicator.SetViewPager(_pager);
                circleIndicator.Snap = true;
                circleIndicator.PageColor = new Color(ContextCompat.GetColor(this, Resource.Color.welcomeBackground));
                circleIndicator.StrokeColor = new Color(ContextCompat.GetColor(this, Resource.Color.colorBlue));
            }
        }
    }
}
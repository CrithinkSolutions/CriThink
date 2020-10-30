using Cirrious.FluentLayouts.Touch;
using CriThink.Client.Core.ViewModels;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace CriThink.Client.iOS.Views.Users
{
    [MvxRootPresentation(WrapInNavigationController = true)]
    public class WelcomeView : MvxViewController<WelcomeViewModel>
    {
        private UIScrollView _scrollView;
        private UIPageControl _pageControl;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _scrollView = new UIScrollView();
            _scrollView.BackgroundColor = UIColor.Black;

            _pageControl = new UIPageControl();
            _pageControl.TintColor = UIColor.Brown;
            _pageControl.CurrentPageIndicatorTintColor = UIColor.White;

            Add(_scrollView);
            Add(_pageControl);

            ApplyConstraints();

            var set = CreateBindingSet();

            set.Apply();
        }

        private void ApplyConstraints()
        {
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(
                _scrollView.AtLeftOf(View),
                _scrollView.AtTopOf(View),
                _scrollView.AtRightOf(View),
                _scrollView.AtBottomOf(View),

                _pageControl.ToBottomMargin(View),
                _pageControl.WithSameCenterX(View)
            );
        }
    }
}
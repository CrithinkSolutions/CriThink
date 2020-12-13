using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using CriThink.Client.Core.ViewModels;
using FFImageLoading;
using FFImageLoading.Cross;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace CriThink.Client.iOS.Views.Users
{
    [MvxRootPresentation(WrapInNavigationController = false)]
    // ReSharper disable once UnusedMember.Global
    public class WelcomeView : MvxViewController<WelcomeViewModel>
    {
        private const int NumberOfPages = 3;

        private UIScrollView _scrollView;
        private UIPageControl _pageControl;
        private CGRect _frame;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _frame = new CGRect(0, 0, 0, 0);

            _scrollView = new UIScrollView
            {
                PagingEnabled = true,
                ScrollEnabled = true,
                ShowsHorizontalScrollIndicator = false,
                ShowsVerticalScrollIndicator = false
            };
            _scrollView.DecelerationEnded += (sender, args) =>
            {
                var pageNumber = Math.Round(_scrollView.ContentOffset.X / _scrollView.Frame.Size.Width);
                _pageControl.CurrentPage = (int) pageNumber;
            };

            _pageControl = new UIPageControl
            {
                Pages = NumberOfPages,
                CurrentPage = 0
            };
            _pageControl.ValueChanged += (sender, args) =>
            {
                var x = _pageControl.CurrentPage * _scrollView.Frame.Size.Width;
                _scrollView.SetContentOffset(new CGPoint(x, 0), true);
            };

            Add(_scrollView);
            Add(_pageControl);

            AddPresentationViews();
            AddSignInView();

            ApplyConstraints();

            _scrollView.ContentSize = new CGSize(View.Frame.Size.Width * NumberOfPages, View.Frame.Size.Height);

            var set = CreateBindingSet();

            set.Apply();
        }

        private void AddPresentationViews()
        {
            var firstView = GetViewForPageControl(0);
            BuildPresentationView(firstView, "signup_background.jpg");

            var secondView = GetViewForPageControl(1);
            BuildPresentationView(secondView, "signup_background.jpg");

            AddInnerContentToScrollView(firstView);
            AddInnerContentToScrollView(secondView);
        }

        private void AddSignInView()
        {
            var index = NumberOfPages - 1;
            var signinView = GetViewForPageControl(index);
            signinView.BackgroundColor = UIColor.White;

            UIImageView imageView = new MvxCachedImageView();
            ImageService.Instance.LoadFile("ic_launcher.png")
                .Into(imageView);

            var btnSignIn = new UIButton();
            btnSignIn.SetTitle("Sign In", UIControlState.Normal);
            btnSignIn.BackgroundColor = UIColor.Gray;

            var btnLogin = new UIButton();
            btnLogin.SetTitle("Login", UIControlState.Normal);
            btnLogin.BackgroundColor = UIColor.Gray;

            signinView.AddSubview(imageView);
            signinView.AddSubview(btnSignIn);
            signinView.AddSubview(btnLogin);
            signinView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            signinView.AddConstraints(
                imageView.AtTopOf(signinView, 40),
                imageView.WithSameCenterX(signinView),
                imageView.Width().EqualTo(120),
                imageView.Height().EqualTo(120),

                btnSignIn.AtBottomOf(signinView, 24),
                btnSignIn.AtLeftOf(signinView, 16),
                btnSignIn.ToLeftOf(btnLogin, 16),

                btnLogin.WithSameBottom(btnSignIn),
                btnLogin.AtRightOf(signinView, 16),
                btnLogin.WithSameWidth(btnSignIn)
                );

            AddInnerContentToScrollView(signinView);
        }

        private UIView GetViewForPageControl(int index)
        {
            _frame.X = View.Frame.Size.Width * index;
            _frame.Size = View.Frame.Size;

            return new UIView(_frame);
        }

        private void BuildPresentationView(UIView parentView, string imageName)
        {
            UIImageView imageView = new MvxCachedImageView();
            ImageService.Instance.LoadFile(imageName)
                .Into(imageView);

            parentView.AddSubview(imageView);
            parentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            parentView.AddConstraints(
                imageView.AtTopOf(parentView, 10),
                imageView.AtRightOf(parentView, 10),
                imageView.AtLeftOf(parentView, 10),
                imageView.AtBottomOf(parentView, 10)
            );
        }

        private void AddInnerContentToScrollView(UIView view)
        {
            _scrollView.AddSubview(view);
        }

        private void ApplyConstraints()
        {
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(
                _scrollView.AtLeftOf(View),
                _scrollView.AtTopOf(View),
                _scrollView.AtRightOf(View),
                _scrollView.AtBottomOf(View),

                _pageControl.AtBottomOf(View, 45),
                _pageControl.WithSameWidth(View),
                _pageControl.WithSameCenterX(View)
            );
        }
    }
}
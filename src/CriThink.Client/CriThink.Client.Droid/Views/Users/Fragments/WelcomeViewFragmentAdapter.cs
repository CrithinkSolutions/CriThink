using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using AndroidX.Fragment.App;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Views.ViewPager;
using MvvmCross.ViewModels;

#pragma warning disable CS0618 // Type or member is obsolete
// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    public class WelcomeViewFragmentAdapter : FragmentPagerAdapter
    {
        private readonly Context _context;

        public WelcomeViewFragmentAdapter(Context context, FragmentManager fragmentManager, IEnumerable<MvxViewPagerFragmentInfo> fragments)
            : base(fragmentManager)
        {
            _context = context;
            Fragments = fragments;
        }

        public IEnumerable<MvxViewPagerFragmentInfo> Fragments { get; private set; }

        public override int Count => Fragments.Count();

        public override Fragment GetItem(int position)
        {
            var mvxViewPagerFragmentInfo = Fragments.ElementAt(position);

#pragma warning disable CS0618 // Type or member is obsolete
            var fragment = Fragment.Instantiate(_context, FragmentJavaName(mvxViewPagerFragmentInfo.FragmentType));
#pragma warning restore CS0618 // Type or member is obsolete

            switch (fragment)
            {
                case WelcomeFragment welcomeFragment:
                    GetImageForWelcomeFragment(welcomeFragment, position);
                    break;
                case MvxFragment mvxFragment:
                    SetViewModelForMvxFragment(mvxFragment, mvxViewPagerFragmentInfo);
                    break;
            }

            return fragment;
        }

        private static void GetImageForWelcomeFragment(WelcomeFragment welcomeFragment, int position)
        {
            switch (position)
            {
                case 0:
                    welcomeFragment.ImageId = Resource.Drawable.signup_background;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void SetViewModelForMvxFragment(MvxFragment mvxFragment, MvxViewPagerFragmentInfo mvxViewPagerFragmentInfo)
        {
            mvxFragment.DataContext = (mvxViewPagerFragmentInfo.Request as MvxViewModelInstanceRequest)?.ViewModelInstance;
        }

        protected virtual string FragmentJavaName(Type fragmentType)
        {
            var namespaceText = fragmentType.Namespace ?? "";
            if (namespaceText.Length > 0)
                namespaceText = namespaceText.ToLowerInvariant() + ".";
            return namespaceText + fragmentType.Name;
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int p0) { return new Java.Lang.String(Fragments.ElementAt(p0).Title); }
    }
}
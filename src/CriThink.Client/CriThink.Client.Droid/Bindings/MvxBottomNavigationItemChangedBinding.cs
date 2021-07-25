using System;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Navigation;
using MvvmCross.Binding;
using MvvmCross.Commands;
using MvvmCross.Platforms.Android.Binding.Target;

namespace CriThink.Client.Droid.Bindings
{
    public class MvxBottomNavigationItemChangedBinding : MvxAndroidTargetBinding
    {
        public const string BindingKey = "BottomNavigationSelectedBindingKey";

        private readonly BottomNavigationView _bottomNav;
        private IMvxCommand _command;

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override Type TargetType => typeof(MvxCommand);

        public MvxBottomNavigationItemChangedBinding(BottomNavigationView bottomNav) : base(bottomNav)
        {
            _bottomNav = bottomNav;
            _bottomNav.ItemSelected += OnNavigationItemSelected;
        }

        public override void SetValue(object value)
        {
            _command = (IMvxCommand) value;
        }

        protected override void SetValueImpl(object target, object value) { }

        private void OnNavigationItemSelected(object sender, NavigationBarView.ItemSelectedEventArgs e)
        {
            _command?.Execute(e.P0.TitleCondensedFormatted.ToString());
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
                _bottomNav.ItemSelected -= OnNavigationItemSelected;

            base.Dispose(isDisposing);
        }
    }
}
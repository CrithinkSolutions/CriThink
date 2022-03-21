using CriThink.Client.Core.ViewModels.Users;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    public abstract class BaseSocialLoginActivity<TViewModel>
        : MvxActivity<TViewModel> where TViewModel : BaseSocialLoginViewModel, IMvxViewModel
    { }
}
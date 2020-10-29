using System;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Client.Droid.Constants;
using Google.Android.Material.TextField;
using MvvmCross;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [IntentFilter(
        actions: new[] { Android.Content.Intent.ActionView },
        Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
        DataSchemes = new[] { DeepLinkConstants.SchemaHTTP, DeepLinkConstants.SchemaHTTPS },
        DataHost = DeepLinkConstants.SchemaHost,
        DataPathPrefix = "/" + DeepLinkConstants.SchemaPrefixResetPassword,
        AutoVerify = false)]
    [MvxActivityPresentation]
    [Activity(LaunchMode = LaunchMode.SingleTop, ClearTaskOnLaunch = true)]
    public class ResetPasswordView : MvxActivity<ResetPasswordViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.resetpassword_view);

            if (ViewModel == null)
            {
                try
                {
                    RestartViewModelWithQueryData();
                }
                catch (Exception)
                {
                    //TODO: log
                }
            }

            var txtPassword = FindViewById<TextInputEditText>(Resource.Id.txtEditPassword);
            var txtRepeatPassword = FindViewById<TextInputEditText>(Resource.Id.txtEditRepeatPassword);
            var btnSend = FindViewById<AppCompatButton>(Resource.Id.btnSend);

            var set = CreateBindingSet();

            set.Bind(txtPassword).To(vm => vm.Password);
            set.Bind(txtRepeatPassword).To(vm => vm.RepeatPassword);

            set.Bind(btnSend).To(vm => vm.SendRequestCommand);

            set.Apply();
        }

        private void RestartViewModelWithQueryData()
        {
            var intentData = Intent?.Data;
            var queryParameterNames = Intent?.Data?.QueryParameterNames;
            if (intentData == null || queryParameterNames == null || !queryParameterNames.Any())
                throw new InvalidOperationException("No data has been provided");

            var data = queryParameterNames.ToDictionary(k => k, v => intentData.GetQueryParameter(v));

            var mvxBundle = new MvxBundle(data);
            var loaderService = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
            ViewModel = (ResetPasswordViewModel) loaderService.LoadViewModel(new MvxViewModelRequest(typeof(ResetPasswordViewModel), mvxBundle, null), null);
        }
    }
}
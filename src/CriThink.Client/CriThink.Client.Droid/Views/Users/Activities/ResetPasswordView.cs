using System;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Client.Droid.Constants;
using CriThink.Client.Droid.Controls;
using Google.Android.Material.TextField;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
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
    [Activity(LaunchMode = LaunchMode.SingleTop, ClearTaskOnLaunch = true, Exported = true)]
    public class ResetPasswordView : MvxActivity<ResetPasswordViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.resetpassword_view);
            MainApplication.SetGradientStatusBar(this);
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

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);

            var txtTitle = FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            var txtInputPassword = FindViewById<TextInputLayout>(Resource.Id.txtInputPassword);
            var txtInputRepeatPassword = FindViewById<TextInputLayout>(Resource.Id.txtInputRepeatPassword);
            var txtPassword = FindViewById<BindableEditText>(Resource.Id.txtEditPassword);
            var txtRepeatPassword = FindViewById<BindableEditText>(Resource.Id.txtEditRepeatPassword);
            var btnSend = FindViewById<AppCompatButton>(Resource.Id.btnSend);
            var loader = FindViewById<LoaderView>(Resource.Id.loader);

            var set = CreateBindingSet();

            set.Bind(txtTitle).ToLocalizationId("ResetPassword");
            set.Bind(txtInputPassword).For(v => v.Hint).ToLocalizationId("InsertPassword");
            set.Bind(txtInputRepeatPassword).For(v => v.Hint).ToLocalizationId("RepeatPassword");
            set.Bind(txtPassword).To(vm => vm.Password);
            set.Bind(txtPassword).For(v => v.KeyCommand).To(vm => vm.SendRequestCommand);
            set.Bind(txtRepeatPassword).To(vm => vm.RepeatPassword);
            set.Bind(txtRepeatPassword).For(v => v.KeyCommand).To(vm => vm.SendRequestCommand);

            set.Bind(btnSend).For(v => v.Text).ToLocalizationId("Send");
            set.Bind(btnSend).To(vm => vm.SendRequestCommand);

            set.Bind(loader).For(v => v.BindVisible()).To(vm => vm.IsLoading);

            set.Apply();
        }

        public override bool OnSupportNavigateUp()
        {
            Finish();
            return false;
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
using Android.App;
using Android.OS;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Client.Droid.Controls;
using Google.Android.Material.TextField;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [MvxActivityPresentation]
    [Activity(Label = "CriThink.ForgotPasswordView")]
    public class ForgotPasswordView : MvxActivity<ForgotPasswordViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.forgotpassword_view);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            MainApplication.SetGradientStatusBar(this);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);

            var email = FindViewById<BindableEditText>(Resource.Id.txtEdit_email);
            var btnSend = FindViewById<AppCompatButton>(Resource.Id.btnSend);
            var txtInputEmail = FindViewById<TextInputLayout>(Resource.Id.txtInput_email);
            var txtTitle = FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            var loader = FindViewById<LoaderView>(Resource.Id.loader);

            var set = CreateBindingSet();

            set.Bind(email).To(vm => vm.EmailOrUsername);
            set.Bind(email).For(v => v.KeyCommand).To(vm => vm.SendRequestCommand);
            set.Bind(txtInputEmail).For(v => v.Hint).ToLocalizationId("InsertEmail");

            set.Bind(btnSend).To(vm => vm.SendRequestCommand);
            set.Bind(btnSend).For(v => v.Text).ToLocalizationId("Send");

            set.Bind(loader).For(v => v.BindVisible()).To(vm => vm.IsLoading);

            set.Bind(txtTitle).ToLocalizationId("RecoverPassword");

            set.Apply();
        }

        public override bool OnSupportNavigateUp()
        {
            Finish();
            return false;
        }
    }
}
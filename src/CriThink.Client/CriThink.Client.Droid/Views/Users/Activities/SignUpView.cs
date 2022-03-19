using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using FFImageLoading.Cross;
using Google.Android.Material.Dialog;
using Google.Android.Material.TextField;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [MvxActivityPresentation]
    [Activity(Label = "CriThink.SignUpView")]
    public class SignUpView : BaseSocialLoginActivity<SignUpViewModel>
    {
        private AppCompatButton _btnFb;
        private AppCompatButton _btnGoogle;
        private AlertDialog _alertDialog;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.signup_view);
            MainApplication.SetGradientStatusBar(this);

            _btnFb = FindViewById<AppCompatButton>(Resource.Id.btnFb);
            if (_btnFb != null)
                _btnFb.Click += BtnFacebook_Click;

            _btnGoogle = FindViewById<AppCompatButton>(Resource.Id.btnGoogle);
            //if (_btnGoogle != null)
            //    _btnGoogle.Click += BtnGoogle_Click;

            var btnSignUpEmail = FindViewById<AppCompatButton>(Resource.Id.btnSignUp);
            var btnLogin = FindViewById<AppCompatButton>(Resource.Id.btnLogin);
            var txtTitle = FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            var txtCaption = FindViewById<AppCompatTextView>(Resource.Id.txtCaption);
            var alreadyAccount = FindViewById<AppCompatTextView>(Resource.Id.alreadyAccount);
            var txtOrAccount = FindViewById<AppCompatTextView>(Resource.Id.txtOrAccount);
            var btnRestore = FindViewById<AppCompatButton>(Resource.Id.btnRestore);
            var loader = FindViewById<MvxCachedImageView>(Resource.Id.loader);

            btnRestore.Click += BtnRestore_Click;

            var set = CreateBindingSet();
            set.Bind(txtTitle).ToLocalizationId("SignUpTitle");
            set.Bind(txtCaption).ToLocalizationId("SignUpCaption");
            set.Bind(btnSignUpEmail).To(vm => vm.NavigateToSignUpEmailCommand);
            set.Bind(btnSignUpEmail).For(v => v.Text).ToLocalizationId("SignUpEmail");
            set.Bind(btnLogin).To(vm => vm.NavigateToLoginCommand);
            set.Bind(btnLogin).For(v => v.Text).ToLocalizationId("Login");
            set.Bind(_btnGoogle).For(v => v.Text).ToLocalizationId("Google");
            set.Bind(_btnGoogle).To(vm => vm.DoGoogleLoginCommand);
            set.Bind(_btnFb).For(v => v.Text).ToLocalizationId("Facebook");
            set.Bind(alreadyAccount).ToLocalizationId("AlreadyAccount");
            set.Bind(txtOrAccount).ToLocalizationId("OrAccount");
            set.Bind(btnRestore).For(v => v.Text).ToLocalizationId("RestoreAccount");
            set.Bind(loader).For(v => v.BindVisible()).To(vm => vm.IsLoading);
            _alertDialog = BuildCustomDialog(set);
            set.Apply();
        }

        private void BtnGoogle_Click(object sender, EventArgs e) => LoginUsingGoogle();

        private void BtnFacebook_Click(object sender, EventArgs e) => LoginUsingFacebook();

        private void BtnRestore_Click(object sender, EventArgs e) => _alertDialog?.Show();

        public override void OnBackPressed()
        {
            Minimise();
        }

        private void Minimise()
        {
            var minimiseIntent = new Intent(Intent.ActionMain);
            minimiseIntent.AddCategory(Intent.CategoryHome);
            minimiseIntent.SetFlags(ActivityFlags.NewTask);
            StartActivity(minimiseIntent);
        }
        private AlertDialog BuildCustomDialog(MvxFluentBindingDescriptionSet<IMvxAndroidView<SignUpViewModel>, SignUpViewModel> set)
        {
            var builder = new MaterialAlertDialogBuilder(this, Resource.Style.MyThemeOverlay_MaterialComponents_MaterialAlertDialog);
            ViewGroup viewGroup = FindViewById<ViewGroup>(Resource.Id.root);
            View dialogView = LayoutInflater.From(this).Inflate(Resource.Layout.dialog_recovery_password, viewGroup, false);
            var title = dialogView.FindViewById<AppCompatTextView>(Resource.Id.tv_header_dialog);
            var content = dialogView.FindViewById<AppCompatTextView>(Resource.Id.tv_content_dialog);
            var inputText = dialogView.FindViewById<TextInputEditText>(Resource.Id.txtInput_emailOrUsername);
            var inputTextLayout = dialogView.FindViewById<TextInputLayout>(Resource.Id.txtEdit_emailOrUsername);
            var btnOk = dialogView.FindViewById<AppCompatButton>(Resource.Id.btn_ok);
            var btnCancel = dialogView.FindViewById<AppCompatButton>(Resource.Id.btn_cancel);
            set.Bind(title).ToLocalizationId("RestoreAccountTitle");
            set.Bind(content).ToLocalizationId("RestoreAccountMessage");
            set.Bind(inputTextLayout).For(v => v.PlaceholderText).ToLocalizationId("EmailHint");
            set.Bind(btnOk).For(v => v.Text).ToLocalizationId("DialogOk");
            set.Bind(btnCancel).For(v => v.Text).ToLocalizationId("DialogCancel");
            dialogView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            builder.SetView(dialogView);
            var alertDialog = builder.Create();
            set.Bind(btnOk).For(v => v.BindClick()).To(vm => vm.RestoreAccountCommand).CommandParameter(inputText.Text);
            btnCancel.Click += (sender, e) => alertDialog.Dismiss();
            btnOk.Click += async (sender, e) =>
            {
                alertDialog.Dismiss();
                await ViewModel.RestoreAccountCommand.ExecuteAsync(inputText.Text);
            };
            return alertDialog;
        }
    }
}
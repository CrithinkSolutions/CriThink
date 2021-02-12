#pragma warning disable CA1054 // URI-like parameters should not be strings
#pragma warning disable CA1056 // URI-like properties should not be strings
namespace CriThink.Server.RazorViews.Views.Emails.ConfirmAccount
{
    /// <summary>
    /// ViewModel for the user account creation confirmation email
    /// </summary>
    public class ConfirmAccountEmailViewModel
    {
        public ConfirmAccountEmailViewModel(string confirmEmailUrl)
        {
            ConfirmEmailUrl = confirmEmailUrl;
        }

        public string ConfirmEmailUrl { get; }
    }
}

namespace CriThink.Server.RazorViews.Views.Emails.ConfirmAccount
{
    /// <summary>
    /// ViewModel for the user account creation confirmation email
    /// </summary>
    public class ConfirmAccountEmailViewModel
    {
        public ConfirmAccountEmailViewModel(string confirmEmailUrl, string hostnameUrl)
        {
            ConfirmEmailUrl = confirmEmailUrl;
            HostnameUrl = hostnameUrl;
        }

        public string ConfirmEmailUrl { get; }

        public string HostnameUrl { get; }
    }
}

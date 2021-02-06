namespace CriThink.Server.RazorViews.Views.Emails.ConfirmAccount
{
    /// <summary>
    /// ViewModel for the user account creation confirmation email
    /// </summary>
    public class ConfirmAccountEmailViewModel
    {
        public ConfirmAccountEmailViewModel(string confirmEmailUrl, string hostnameUrl, string userName)
        {
            ConfirmEmailUrl = confirmEmailUrl;
            HostnameUrl = hostnameUrl;
            UserName = userName;
        }

        public string ConfirmEmailUrl { get; }

        public string HostnameUrl { get; }

        public string UserName { get; }
    }
}

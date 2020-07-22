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

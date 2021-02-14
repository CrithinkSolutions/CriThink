namespace CriThink.Server.RazorViews.Views.Emails.AlertNotification
{
    public class UnknownDomainAlertViewModel
    {
        public UnknownDomainAlertViewModel(string unknownDomainUrl, string userEmail)
        {
            UnknownDomainUrl = unknownDomainUrl;
            UserEmail = userEmail;
        }

        public string UserEmail { get; }

        public string UnknownDomainUrl { get; }
    }
}

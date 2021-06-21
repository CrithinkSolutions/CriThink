namespace CriThink.Server.RazorViews.Views.Emails.AlertNotification
{
    public class UnknownDomainAdminAlertViewModel
    {
        public UnknownDomainAdminAlertViewModel(string unknownDomain, string userEmail)
        {
            UnknownDomain = unknownDomain;
            UserEmail = userEmail;
        }

        public string UserEmail { get; }

        public string UnknownDomain { get; }
    }
}

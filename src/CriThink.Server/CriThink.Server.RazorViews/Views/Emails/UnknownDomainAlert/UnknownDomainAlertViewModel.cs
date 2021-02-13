namespace CriThink.Server.RazorViews.Views.Emails.AlertNotification
{
    public class UnknownDomainAlertViewModel
    {
        public UnknownDomainAlertViewModel(string unknownDomainUrl)
        {
            UnknownDomainUrl = unknownDomainUrl;
        }

        public string UnknownDomainUrl { get; }
    }
}

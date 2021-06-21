namespace CriThink.Server.RazorViews.Views.Emails.AlertNotification
{
    public class UnknownDomainAdminUserAlertViewModel
    {
        public UnknownDomainAdminUserAlertViewModel(string unknownDomain, string classification)
        {
            UnknownDomain = unknownDomain;
            Classification = classification;
        }

        public string UnknownDomain { get; }


        public string Classification { get; }
    }
}
namespace CriThink.Server.RazorViews.Views.Emails.AlertNotification
{
    public class UnknownDomainAdminUserAlertViewModel
    {
        public UnknownDomainAdminUserAlertViewModel(string unknownDomainUrl, string classification)
        {
            UnknownDomainUrl = unknownDomainUrl;
            Classification = classification;
        }

        public string UnknownDomainUrl { get; }


        public string Classification { get; }
    }
}
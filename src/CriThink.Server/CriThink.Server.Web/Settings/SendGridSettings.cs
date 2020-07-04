namespace CriThink.Server.Web.Settings
{
    /// <summary>
    /// SendGrid email provider settings
    /// </summary>
    public class SendGridSettings
    {
        public string User { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string ConfirmationEmailSubject { get; set; }
        public string ConfirmationEmailLink { get; set; }
    }
}

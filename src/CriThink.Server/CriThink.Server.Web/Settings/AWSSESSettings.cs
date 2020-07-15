namespace CriThink.Server.Web.Settings
{
    /// <summary>
    /// AWS SES settings
    /// </summary>
    public class AWSSESSettings
    {
        public string FromAddress { get; set; }
        public string ConfirmationEmailSubject { get; set; }
        public string ConfirmationEmailLink { get; set; }
    }
}

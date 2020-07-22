namespace CriThink.Server.Providers.EmailSender.Settings
{
    public class AwsSESSettings
    {
        public string FromAddress { get; set; }
        public string ConfirmationEmailSubject { get; set; }
        public string ConfirmationEmailLink { get; set; }
    }
}

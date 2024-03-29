﻿namespace CriThink.Server.Providers.EmailSender.Settings
{
    public class EmailSettings
    {
        public string FromAddress { get; set; }
        public string ConfirmationEmailSubject { get; set; }
        public string ForgotPasswordSubject { get; set; }
        public string ConfirmationEmailLink { get; set; }
        public string ForgotPasswordLink { get; set; }
        public string AdminEmailAddress { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace CriThink.Server.Providers.EmailSender.Exceptions
{
    public class CriThinkEmailSendingFailureException : Exception
    {
        public CriThinkEmailSendingFailureException(
            Exception ex,
            string fromAddress,
            IEnumerable<string> recipients,
            string subject,
            string htmlBody)
            : base(ex?.Message)
        {
            Ex = ex;
            FromAddress = fromAddress;
            Recipients = recipients;
            Subject = subject;
            HtmlBody = htmlBody;
        }

        public Exception Ex { get; }

        public string FromAddress { get; }

        public IEnumerable<string> Recipients { get; }

        public string Subject { get; }

        public string HtmlBody { get; }
    }
}

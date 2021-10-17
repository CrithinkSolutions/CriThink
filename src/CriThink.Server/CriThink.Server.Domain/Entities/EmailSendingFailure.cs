using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CriThink.Server.Domain.Entities
{
    public class EmailSendingFailure : Entity<Guid>, IAggregateRoot
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected EmailSendingFailure()
        { }

        private EmailSendingFailure(
            string fromAddress,
            IEnumerable<string> recipients,
            string htmlBody,
            string subject,
            string exceptionMessage)
        {
            FromAddress = fromAddress;
            Recipients = new List<string>(recipients);
            HtmlBody = htmlBody;
            Subject = subject;
            ExceptionMessage = exceptionMessage;
        }

        public string FromAddress { get; }

        public ICollection<string> Recipients { get; }

        public string HtmlBody { get; }

        public string Subject { get; }

        public string ExceptionMessage { get; }

        #region Create

        public static EmailSendingFailure Create(
            string fromAddress,
            IEnumerable<string> recipients,
            string htmlBody,
            string subject,
            string exceptionMessage)
        {
            return new EmailSendingFailure(
                fromAddress,
                recipients,
                htmlBody,
                subject,
                exceptionMessage);
        }

        #endregion
    }
}

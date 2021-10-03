using System;
using System.Diagnostics.CodeAnalysis;

namespace CriThink.Server.Domain.Entities
{
    public class DebunkingNewsTriggerLog : Entity<Guid>, IAggregateRoot
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected DebunkingNewsTriggerLog()
        { }

        private DebunkingNewsTriggerLog(DebunkingNewsTriggerLogStatus status)
        {
            Status = status;
            TimeStamp = DateTime.UtcNow;
        }

        private DebunkingNewsTriggerLog(
            DebunkingNewsTriggerLogStatus status,
            string message)
            : this(status)
        {
            Failures = message;
        }

        public DebunkingNewsTriggerLogStatus Status { get; private set; }

        public DateTimeOffset TimeStamp { get; private set; }

        public string Failures { get; private set; }

        #region Create

        public static DebunkingNewsTriggerLog CreateWithSuccess()
        {
            return new DebunkingNewsTriggerLog(DebunkingNewsTriggerLogStatus.Successfull);
        }

        public static DebunkingNewsTriggerLog CreateWithPartialFailure(string failures)
        {
            return new DebunkingNewsTriggerLog(DebunkingNewsTriggerLogStatus.Partial, failures);
        }

        public static DebunkingNewsTriggerLog CreateWithFailure(string failures)
        {
            return new DebunkingNewsTriggerLog(DebunkingNewsTriggerLogStatus.Failed, failures);
        }

        #endregion
    }
}

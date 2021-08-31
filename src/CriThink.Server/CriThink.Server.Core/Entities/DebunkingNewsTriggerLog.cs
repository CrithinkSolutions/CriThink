using System;
using System.Diagnostics.CodeAnalysis;

namespace CriThink.Server.Core.Entities
{
    public class DebunkingNewsTriggerLog : Entity<Guid>
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected DebunkingNewsTriggerLog()
        { }

        private DebunkingNewsTriggerLog(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
            TimeStamp = DateTime.UtcNow;
        }

        private DebunkingNewsTriggerLog(bool isSuccessful, string message)
            : this(isSuccessful)
        {
            FailReason = message;
        }

        public bool IsSuccessful { get; private set; }

        public DateTimeOffset TimeStamp { get; private set; }

        public string FailReason { get; private set; }

        public static DebunkingNewsTriggerLog Create()
        {
            return new DebunkingNewsTriggerLog(true);
        }

        public static DebunkingNewsTriggerLog Create(string message)
        {
            return new DebunkingNewsTriggerLog(false, message);
        }
    }
}

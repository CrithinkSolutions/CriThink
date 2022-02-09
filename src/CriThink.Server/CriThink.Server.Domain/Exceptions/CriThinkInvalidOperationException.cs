using System;

namespace CriThink.Server.Domain.Exceptions
{
    public class CriThinkInvalidOperationException : Exception
    {
        public CriThinkInvalidOperationException(
            string message)
            : base(message)
        { }
    }
}

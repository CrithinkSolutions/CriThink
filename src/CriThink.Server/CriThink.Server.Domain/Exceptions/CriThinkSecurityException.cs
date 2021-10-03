using System;

namespace CriThink.Server.Domain.Exceptions
{
    public class CriThinkSecurityException : Exception
    {
        public CriThinkSecurityException(string message)
            : base(message)
        {

        }
    }
}

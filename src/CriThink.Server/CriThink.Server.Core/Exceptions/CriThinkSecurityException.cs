using System;

namespace CriThink.Server.Core.Exceptions
{
    public class CriThinkSecurityException : Exception
    {
        public CriThinkSecurityException(string message)
            : base(message)
        {

        }
    }
}

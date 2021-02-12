using System;

namespace CriThink.Server.Core.Exceptions
{
    public class SecretNotFoundException : Exception
    {
        public SecretNotFoundException(string message, string component = null)
            : base($"Secret not available: {message}. {component}")
        { }
    }
}

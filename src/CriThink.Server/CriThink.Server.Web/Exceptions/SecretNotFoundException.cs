using System;

#pragma warning disable CA1032 // Implement standard exception constructors

namespace CriThink.Server.Web.Exceptions
{
    public class SecretNotFoundException : Exception
    {
        public SecretNotFoundException(string message, string component = null)
            : base($"Secret not available: {message}. {component}")
        { }
    }
}

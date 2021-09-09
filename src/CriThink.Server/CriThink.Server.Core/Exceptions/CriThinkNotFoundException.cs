using System;

namespace CriThink.Server.Core.Exceptions
{
    /// <summary>
    /// Custom exception to throw when the given Id doesn't exist
    /// </summary>
    public class CriThinkNotFoundException : Exception
    {
        public CriThinkNotFoundException(string message, string resource = null)
            : base($"{message}. {resource}")
        { }

        public CriThinkNotFoundException(string message, Guid id)
            : base($"{message}. {id}")
        { }
    }
}

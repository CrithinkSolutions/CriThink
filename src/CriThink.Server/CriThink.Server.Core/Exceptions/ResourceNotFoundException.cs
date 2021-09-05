using System;

namespace CriThink.Server.Core.Exceptions
{
    /// <summary>
    /// Custom exception to throw when the given Id doesn't exist
    /// </summary>
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message, string resource = null)
            : base($"{message}. {resource}")
        { }

        public ResourceNotFoundException(string message, Guid id)
            : base($"{message}. {id}")
        { }
    }
}

using System;
using System.Collections.Generic;

namespace CriThink.Server.Domain.Exceptions
{
    public class CriThinkBaseException : Exception
    {
        public CriThinkBaseException(int code, string message)
            : base(message)
        {
            Code = code;
        }

        public CriThinkBaseException(int code, string message, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }

        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public int Code { get; }
    }
}

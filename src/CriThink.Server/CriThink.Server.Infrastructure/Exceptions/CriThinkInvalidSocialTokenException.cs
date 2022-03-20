using CriThink.Server.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Infrastructure.Exceptions
{
    internal class CriThinkInvalidSocialTokenException : CriThinkBaseException
    {
        public CriThinkInvalidSocialTokenException()
            : base(StatusCodes.Status500InternalServerError, "The given token is invalid")
        { }
    }
}

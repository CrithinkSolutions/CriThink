using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Domain.Exceptions
{
    public class CriThinkAlreadyAnsweredException : CriThinkBaseException
    {
        public CriThinkAlreadyAnsweredException()
            : base(StatusCodes.Status409Conflict, "This user has already gave a rate for this news")
        {

        }
    }
}

using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class DeleteNewsSourceCommand : IRequest
    {
        public DeleteNewsSourceCommand(string newsSource)
        {
            NewsSource = newsSource;
        }

        public string NewsSource { get; }
    }
}

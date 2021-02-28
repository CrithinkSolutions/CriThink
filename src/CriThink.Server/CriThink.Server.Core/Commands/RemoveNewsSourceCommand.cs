using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class RemoveNewsSourceCommand : IRequest
    {
        public RemoveNewsSourceCommand(string newsLink)
        {
            NewsLink = newsLink;
        }

        public string NewsLink { get; }
    }
}

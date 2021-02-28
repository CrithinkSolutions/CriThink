using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class CreateUnknownNewsSourceCommand : IRequest
    {
        public CreateUnknownNewsSourceCommand(string newsLink)
        {
            NewsLink = newsLink;
        }

        public string NewsLink { get; }
    }
}

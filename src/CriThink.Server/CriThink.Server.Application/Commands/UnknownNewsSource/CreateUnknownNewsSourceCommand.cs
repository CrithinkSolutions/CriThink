using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class CreateUnknownNewsSourceCommand : IRequest
    {
        public CreateUnknownNewsSourceCommand(
            string newsLink)
        {
            NewsLink = newsLink;
        }

        public string NewsLink { get; }
    }
}

using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class CreateNewsSourceCommand : IRequest
    {
        public CreateNewsSourceCommand(string newsLink, NewsSourceAuthenticity authencity)
        {
            NewsLink = newsLink;
            Authencity = authencity;
        }

        public string NewsLink { get; }

        public NewsSourceAuthenticity Authencity { get; }
    }
}

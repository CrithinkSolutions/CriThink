using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class CreateNewsSourceCommand : IRequest
    {
        public CreateNewsSourceCommand(
            string newsLink,
            NewsSourceAuthenticity newsSourceClassification)
        {
            NewsLink = newsLink;
            NewsSourceClassification = newsSourceClassification;
        }

        public string NewsLink { get; }

        public NewsSourceAuthenticity NewsSourceClassification { get; }
    }
}

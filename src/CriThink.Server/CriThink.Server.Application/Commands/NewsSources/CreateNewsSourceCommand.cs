using CriThink.Common.Endpoints.DTOs.NewsSource;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class CreateNewsSourceCommand : IRequest
    {
        public CreateNewsSourceCommand(
            string newsLink,
            NewsSourceClassification newsSourceClassification)
        {
            NewsLink = newsLink;
            NewsSourceClassification = newsSourceClassification;
        }

        public string NewsLink { get; }

        public NewsSourceClassification NewsSourceClassification { get; }
    }
}

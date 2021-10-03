using CriThink.Server.Domain.Entities;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class IdentifyUnknownNewsSourceCommand : IRequest
    {
        public IdentifyUnknownNewsSourceCommand(
            string source, NewsSourceAuthenticity classification)
        {
            Source = source;
            Classification = classification;
        }

        public string Source { get; }

        public NewsSourceAuthenticity Classification { get; }
    }
}

using System;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class UpdateIdentifiedNewsSourceCommand : IRequest
    {
        public UpdateIdentifiedNewsSourceCommand(Guid newsSourceId, NewsSourceAuthenticity authenticity)
        {
            NewsSourceId = newsSourceId;
            Authenticity = authenticity;
        }

        public Guid NewsSourceId { get; }
        public NewsSourceAuthenticity Authenticity { get; set; }
    }
}

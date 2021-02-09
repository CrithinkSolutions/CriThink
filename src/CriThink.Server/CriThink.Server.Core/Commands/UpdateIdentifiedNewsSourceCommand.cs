using System;

namespace CriThink.Server.Core.Commands
{
    public class UpdateIdentifiedNewsSourceCommand
    {
        public UpdateIdentifiedNewsSourceCommand(Guid newsSourceId)
        {
            NewsSourceId = newsSourceId;
        }

        public Guid NewsSourceId { get; }
    }
}

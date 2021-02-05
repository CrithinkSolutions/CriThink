using System;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class CreateUnknownSourceNotificationRequestCommand : IRequest
    {
        public CreateUnknownSourceNotificationRequestCommand(Guid newsSourceId, string email)
        {
            NewsSourceId = newsSourceId;
            Email = email;
        }

        public Guid NewsSourceId { get; set; }

        public string Email { get; set; }
    }
}

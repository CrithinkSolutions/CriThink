using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.DomainEvents
{
    public class NewsSourceNotificationCreateDomainEvent : INotification
    {
        public NewsSourceNotificationCreateDomainEvent(
            string userEmail,
            UnknownNewsSource unknownNewsSource)
        {
            UserEmail = userEmail;
            UnknownNewsSource = unknownNewsSource;
        }

        public string UserEmail { get; }

        public UnknownNewsSource UnknownNewsSource { get; }
    }
}

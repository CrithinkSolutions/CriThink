using CriThink.Server.Domain.Entities;
using MediatR;

namespace CriThink.Server.Domain.DomainEvents
{
    public class UpdateNewsSourceRepositoryDomainEvent : INotification
    {
        public UpdateNewsSourceRepositoryDomainEvent(
            string domain,
            NewsSourceAuthenticity authenticity)
        {
            Domain = domain;
            Authenticity = authenticity;
        }

        public string Domain { get; }

        public NewsSourceAuthenticity Authenticity { get; }
    }
}

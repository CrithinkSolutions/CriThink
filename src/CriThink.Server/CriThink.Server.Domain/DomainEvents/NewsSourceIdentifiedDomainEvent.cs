using System;
using CriThink.Server.Domain.Entities;
using MediatR;

namespace CriThink.Server.Domain.DomainEvents
{
    public class NewsSourceIdentifiedDomainEvent : INotification
    {
        public NewsSourceIdentifiedDomainEvent(
            string email,
            UnknownNewsSource unknownNewsSource,
            DateTimeOffset requestedAt)
        {
            Email = email;
            UnknownNewsSource = unknownNewsSource;
            RequestedAt = requestedAt;
        }

        public string Email { get; }

        public UnknownNewsSource UnknownNewsSource { get; }

        public DateTimeOffset RequestedAt { get; }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace CriThink.Server.Core.Entities
{
    public abstract class DomainEventsBasedObject
    {
        private readonly List<INotification> _domainEvents = new List<INotification>();

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);

        public void RemoveDomainEvent(INotification eventItem) => _domainEvents.Remove(eventItem);

        public void ClearDomainEvents() => _domainEvents.Clear();

        public async Task RaiseDomainEventsAsync(IMediator mediator)
        {
            var domainEvents = DomainEvents.ToList();
            ClearDomainEvents();
            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}

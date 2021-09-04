using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace CriThink.Server.Core.Entities
{
    public abstract class DomainEventsBasedObject
    {
        private readonly List<INotification> _domainEventsCollection = new List<INotification>();

        public IReadOnlyCollection<INotification> DomainEvents => _domainEventsCollection.AsReadOnly();

        public void AddDomainEvent(INotification eventItem) => _domainEventsCollection.Add(eventItem);

        public void RemoveDomainEvent(INotification eventItem) => _domainEventsCollection.Remove(eventItem);

        public void ClearDomainEvents() => _domainEventsCollection.Clear();

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

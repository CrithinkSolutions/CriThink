using System.Linq;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.ExtensionMethods
{
    internal static class MediatorExtensions
    {
        public static async Task DispatchDomainEventsAsync(
            this IMediator mediator,
            DbContext dbContext)
        {
            var domainEntities = dbContext
                .ChangeTracker
                .Entries<DomainEventsBasedObject>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}

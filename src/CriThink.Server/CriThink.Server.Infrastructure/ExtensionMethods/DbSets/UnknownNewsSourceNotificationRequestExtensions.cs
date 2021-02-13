using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.ExtensionMethods.DbSets
{
    internal static class UnknownNewsSourceNotificationRequestExtensions
    {
        internal static Task<List<GetAllSubscribedUsersResponse>> GetAllSubscribedUsersForUnknownNewsSourceId(
            this DbSet<UnknownNewsSourceNotificationRequest> source,
            Guid unknownNewsSourceId,
            int pageSize,
            int pageIndex,
            Expression<Func<UnknownNewsSourceNotificationRequest, GetAllSubscribedUsersResponse>> projection,
            CancellationToken cancellationToken)
        {
            return source.Where(unsnr => unsnr.UnknownNewsSource.Id == unknownNewsSourceId)
                         .OrderBy(unsnr => unsnr.RequestedAt)
                         .Skip(pageIndex * pageSize)
                         .Take(pageSize)
                         .Select(projection)
                         .ToListAsync(cancellationToken);
        }
    }
}

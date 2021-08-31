﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;

namespace CriThink.Server.Core.Repositories
{
    public interface IUnknownNewsSourcesRepository
    {
        Task<List<GetAllUnknownSourcesQueryResult>> GetAllUnknownSourcesAsync(
            int pageSize,
            int pageIndex,
            CancellationToken cancellationToken = default);

        Task<UnknownNewsSource> GetUnknownNewsSourceByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<Guid> GetUnknownNewsSourceByIdAsync(
            string newsLink,
            CancellationToken cancellationToken = default);

        Task<IList<GetAllSubscribedUsersQueryResult>> GetAllSubscribedUsersAsync(
            Guid unknownNewsId,
            int pageSize,
            int pageIndex,
            CancellationToken cancellationToken = default);
    }
}

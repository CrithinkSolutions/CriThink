using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Application.Queries
{
    public class UnknownNewsSourceQueries : IUnknownNewsSourceQueries
    {
        private readonly CriThinkDbContext _dbContext;

        public UnknownNewsSourceQueries(
            CriThinkDbContext dbContext)
        {
            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<UnknownNewsSource> GetUnknownNewsSourceByUriAsync(string newsSourceLink, CancellationToken cancellationToken)
        {
            var unknownNewsSource = await _dbContext.UnknownNewsSources
               .SingleOrDefaultAsync(uns => uns.Uri == newsSourceLink, cancellationToken);

            return unknownNewsSource;
        }
    }
}

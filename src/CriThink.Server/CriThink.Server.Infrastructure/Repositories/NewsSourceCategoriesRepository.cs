using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;
using CriThink.Server.Infrastructure.Projections;

namespace CriThink.Server.Infrastructure.Repositories
{
    internal class NewsSourceCategoriesRepository : INewsSourceCategoriesRepository
    {
        private readonly CriThinkDbContext _dbContext;

        public NewsSourceCategoriesRepository(
            CriThinkDbContext dbContext)
        {
            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<string> GetDescriptionByCategoryNameAsync(
            NewsSourceAuthenticity category,
            CancellationToken cancellationToken = default)
        {
            var description = await _dbContext.NewsSourceCategories
                .GetCategoryDescriptionByAuthenticityAsync(
                    NewsSourceCategoryProjection.GetDescription,
                    category,
                    cancellationToken);

            return description;
        }
    }
}

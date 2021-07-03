using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class CreateUserSearchHandler : IRequestHandler<CreateUserSearchCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<CreateUserSearchHandler> _logger;

        public CreateUserSearchHandler(CriThinkDbContext dbContext, ILogger<CreateUserSearchHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateUserSearchCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var search = new UserSearch()
                {
                    NewsLink = request.NewsLink,
                    Authenticity = request.Classification,
                    UserId = request.UserId,
                    Timestamp = DateTimeOffset.UtcNow,
                };

                await _dbContext.UserSearches.AddAsync(search, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error adding a news search");
                throw;
            }

            return Unit.Value;
        }
    }
}

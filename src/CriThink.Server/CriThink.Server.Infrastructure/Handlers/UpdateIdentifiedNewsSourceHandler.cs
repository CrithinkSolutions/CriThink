using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    public class UpdateIdentifiedNewsSourceHandler : IRequestHandler<UpdateIdentifiedNewsSourceCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<UpdateIdentifiedNewsSourceHandler> _logger;

        public UpdateIdentifiedNewsSourceHandler(CriThinkDbContext dbContext, ILogger<UpdateIdentifiedNewsSourceHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateIdentifiedNewsSourceCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var newsSource = await _dbContext.UnknownNewsSources
                                                 .SingleOrDefaultAsync(uns => uns.Id == request.NewsSourceId, cancellationToken)
                                                 .ConfigureAwait(false);

                if (newsSource is null)
                    throw new ResourceNotFoundException(nameof(newsSource));

                newsSource.IdentifiedAt = DateTime.UtcNow;
                newsSource.Authenticity = request.Authenticity;

                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

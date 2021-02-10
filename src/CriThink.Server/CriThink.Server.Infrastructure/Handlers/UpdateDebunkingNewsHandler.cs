using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class UpdateDebunkingNewsHandler : IRequestHandler<UpdateDebunkingNewsCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<UpdateDebunkingNewsHandler> _logger;

        public UpdateDebunkingNewsHandler(CriThinkDbContext dbContext, ILogger<UpdateDebunkingNewsHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateDebunkingNewsCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {

                var debunkingNews = await _dbContext.DebunkingNews.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                if (!string.IsNullOrWhiteSpace(request.Caption))
                    debunkingNews.NewsCaption = request.Caption;

                if (!string.IsNullOrWhiteSpace(request.Title))
                    debunkingNews.Title = request.Title;

                if (!string.IsNullOrWhiteSpace(request.Link))
                    debunkingNews.Link = request.Link;

                if (request.Keywords != null && request.Keywords.Any())
                    debunkingNews.Keywords = string.Join(',', request.Keywords);

                if (!string.IsNullOrWhiteSpace(request.ImageLink))
                    debunkingNews.ImageLink = request.ImageLink;

                _dbContext.DebunkingNews.Update(debunkingNews);

                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error updating a debunking news", request);
                throw;
            }

            return Unit.Value;
        }
    }
}

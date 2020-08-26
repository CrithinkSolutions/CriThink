using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    // ReSharper disable once UnusedMember.Global
    internal class CreateQuestionHandler : IRequestHandler<Question>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<CreateQuestionHandler> _logger;

        public CreateQuestionHandler(CriThinkDbContext dbContext, ILogger<CreateQuestionHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(Question request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                await _dbContext.Questions.AddAsync(request, cancellationToken).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error adding question", request);
                throw;
            }

            return Unit.Value;
        }
    }
}

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
    internal class CreateQuestionAnswerHandler : IRequestHandler<QuestionAnswer>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<CreateQuestionAnswerHandler> _logger;

        public CreateQuestionAnswerHandler(CriThinkDbContext dbContext, ILogger<CreateQuestionAnswerHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(QuestionAnswer request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                await _dbContext.QuestionAnswers.AddAsync(request, cancellationToken).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error adding a question answer", request);
                throw;
            }

            return Unit.Value;
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class CreateArticleAnswerHandler : IRequestHandler<CreateArticleAnswerCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<CreateArticleAnswerHandler> _logger;

        public CreateArticleAnswerHandler(CriThinkDbContext dbContext, ILogger<CreateArticleAnswerHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateArticleAnswerCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var entity = request.Answer;

                await _dbContext.ArticleAnswers.AddAsync(entity, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error creating a new article answer");
                throw;
            }

            return Unit.Value;
        }
    }
}

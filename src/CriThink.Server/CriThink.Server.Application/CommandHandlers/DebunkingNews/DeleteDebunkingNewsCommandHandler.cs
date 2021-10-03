using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class DeleteDebunkingNewsCommandHandler : IRequestHandler<DeleteDebunkingNewsCommand>
    {
        private readonly IDebunkingNewsRepository _debunkingNewsRepository;
        private readonly ILogger<DeleteDebunkingNewsCommandHandler> _logger;

        public DeleteDebunkingNewsCommandHandler(
            IDebunkingNewsRepository debunkingNewsRepository,
            ILogger<DeleteDebunkingNewsCommandHandler> logger)
        {
            _debunkingNewsRepository = debunkingNewsRepository ??
                throw new ArgumentNullException(nameof(debunkingNewsRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDebunkingNewsCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(DeleteDebunkingNewsCommandHandler));

            await _debunkingNewsRepository.RemoveDebunkingNewsByIdAsync(request.Id);

            await _debunkingNewsRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            _logger?.LogInformation($"{nameof(DeleteDebunkingNewsCommandHandler)}: done");

            return Unit.Value;
        }
    }
}

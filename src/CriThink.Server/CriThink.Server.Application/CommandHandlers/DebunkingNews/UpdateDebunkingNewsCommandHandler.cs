using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class UpdateDebunkingNewsCommandHandler : IRequestHandler<UpdateDebunkingNewsCommand>
    {
        private readonly IDebunkingNewsRepository _debunkingNewsRepository;
        private readonly ILogger<UpdateDebunkingNewsCommandHandler> _logger;

        public UpdateDebunkingNewsCommandHandler(
            IDebunkingNewsRepository debunkingNewsRepository,
            ILogger<UpdateDebunkingNewsCommandHandler> logger)
        {
            _debunkingNewsRepository = debunkingNewsRepository ??
                throw new ArgumentNullException(nameof(debunkingNewsRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateDebunkingNewsCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(UpdateDebunkingNewsCommandHandler));

            await _debunkingNewsRepository.UpdateDebunkingNewsAsync(
                request.Id,
                request.Title,
                request.Caption,
                request.Link,
                request.Keywords,
                request.ImageLink);

            await _debunkingNewsRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            _logger?.LogInformation($"{nameof(UpdateDebunkingNewsCommandHandler)}: done");

            return Unit.Value;
        }
    }
}

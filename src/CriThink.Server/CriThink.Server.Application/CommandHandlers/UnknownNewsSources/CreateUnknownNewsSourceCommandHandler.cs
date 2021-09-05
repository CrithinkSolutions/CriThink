using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class CreateUnknownNewsSourceCommandHandler : IRequestHandler<CreateUnknownNewsSourceCommand>
    {
        private readonly IUnknownNewsSourcesRepository _unknownNewsSourcesRepository;
        private readonly ILogger<CreateUnknownNewsSourceCommandHandler> _logger;

        public CreateUnknownNewsSourceCommandHandler(
            IUnknownNewsSourcesRepository unknownNewsSourcesRepository,
            ILogger<CreateUnknownNewsSourceCommandHandler> logger)
        {
            _unknownNewsSourcesRepository = unknownNewsSourcesRepository ??
                throw new ArgumentNullException(nameof(unknownNewsSourcesRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(CreateUnknownNewsSourceCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                await _unknownNewsSourcesRepository.AddUnknownNewsSourceAsync(request.NewsLink);

                await _unknownNewsSourcesRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

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

using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Validators;
using CriThink.Server.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class DeleteNewsSourceCommandHandler : IRequestHandler<DeleteNewsSourceCommand>
    {
        private readonly INewsSourceRepository _newsSourceRepository;
        private readonly ILogger<DeleteNewsSourceCommandHandler> _logger;

        public DeleteNewsSourceCommandHandler(
            INewsSourceRepository newsSourceRepository,
            ILogger<DeleteNewsSourceCommandHandler> logger)
        {
            _newsSourceRepository = newsSourceRepository ??
                throw new ArgumentNullException(nameof(newsSourceRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteNewsSourceCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(DeleteNewsSourceCommand));

            var resolver = new DomainValidator();
            var validatedNewsLink = resolver.ValidateDomain(request.NewsSource);

            await _newsSourceRepository.RemoveNewsSourceAsync(validatedNewsLink);

            _logger?.LogInformation($"{nameof(DeleteNewsSourceCommand)}: done");

            return Unit.Value;
        }
    }
}

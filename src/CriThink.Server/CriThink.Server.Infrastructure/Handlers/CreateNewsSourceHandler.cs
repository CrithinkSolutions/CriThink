using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    // ReSharper disable once UnusedMember.Global
    internal class CreateNewsSourceHandler : IRequestHandler<CreateNewsSourceCommand>
    {
        private readonly INewsSourceRepository _newsSourceRepository;
        private readonly ILogger<CreateNewsSourceHandler> _logger;

        public CreateNewsSourceHandler(INewsSourceRepository newsSourceRepository, ILogger<CreateNewsSourceHandler> logger)
        {
            _newsSourceRepository = newsSourceRepository ?? throw new ArgumentNullException(nameof(newsSourceRepository));
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateNewsSourceCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                await _newsSourceRepository.AddNewsSourceAsync(request.Uri, request.Authencity)
                    .ConfigureAwait(false);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a news source", request.Uri, request.Authencity);
                throw;
            }
        }
    }
}

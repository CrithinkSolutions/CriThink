using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Core.DomainServices;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class CreateDebunkingNewsCommandHandler : IRequestHandler<CreateDebunkingNewsCommand>
    {
        private readonly IDebunkingNewsPublisherService _debunkingNewsPublisherService;
        private readonly IDebunkingNewsRepository _debunkingNewsRepository;
        private readonly ILogger<CreateDebunkingNewsCommandHandler> _logger;

        public CreateDebunkingNewsCommandHandler(
            IDebunkingNewsPublisherService debunkingNewsPublisherService,
            IDebunkingNewsRepository debunkingNewsRepository,
            ILogger<CreateDebunkingNewsCommandHandler> logger)
        {
            _debunkingNewsPublisherService = debunkingNewsPublisherService ??
                throw new ArgumentNullException(nameof(debunkingNewsPublisherService));

            _debunkingNewsRepository = debunkingNewsRepository ??
                throw new ArgumentNullException(nameof(debunkingNewsRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(CreateDebunkingNewsCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(CreateDebunkingNewsCommandHandler));

            var debunkingNews = DebunkingNews.Create(
                request.Title,
                request.Caption,
                request.Link,
                request.ImageLink,
                request.Keywords);

            await debunkingNews.SetPublisherAsync(
                _debunkingNewsPublisherService,
                request.PublisherId,
                cancellationToken);

            await _debunkingNewsRepository.AddDebunkingNewsAsync(debunkingNews);

            _logger?.LogInformation($"{nameof(CreateDebunkingNewsCommandHandler)}: done");

            return Unit.Value;
        }
    }
}

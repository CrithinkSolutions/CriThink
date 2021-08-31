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
        private readonly IDebunkingNewsPublisherRepository _debunkingNewsPublisherRepository;
        private readonly ILogger<CreateDebunkingNewsCommandHandler> _logger;

        public CreateDebunkingNewsCommandHandler(
            IDebunkingNewsPublisherService debunkingNewsPublisherService,
            IDebunkingNewsRepository debunkingNewsRepository,
            IDebunkingNewsPublisherRepository debunkingNewsPublisherRepository,
            ILogger<CreateDebunkingNewsCommandHandler> logger)
        {
            _debunkingNewsPublisherService = debunkingNewsPublisherService ??
                throw new ArgumentNullException(nameof(debunkingNewsPublisherService));

            _debunkingNewsRepository = debunkingNewsRepository ??
                throw new ArgumentNullException(nameof(debunkingNewsRepository));

            _debunkingNewsPublisherRepository = debunkingNewsPublisherRepository ??
                throw new ArgumentNullException(nameof(debunkingNewsPublisherRepository));
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

            await debunkingNews.SetPublisherAsync(_debunkingNewsPublisherService, request.PublisherId);

            await _debunkingNewsRepository.AddDebunkingNewsAsync(debunkingNews);

            _logger?.LogInformation($"{nameof(CreateDebunkingNewsCommandHandler)}: done");

            return Unit.Value;
        }
    }
}

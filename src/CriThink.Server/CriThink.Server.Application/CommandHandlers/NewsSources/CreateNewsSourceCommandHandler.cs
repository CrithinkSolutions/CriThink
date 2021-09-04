using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Validators;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class CreateNewsSourceCommandHandler : IRequestHandler<CreateNewsSourceCommand>
    {
        private readonly IMapper _mapper;
        private readonly INewsSourceRepository _newsSourceRepository;
        private readonly ILogger<CreateNewsSourceCommandHandler> _logger;

        public CreateNewsSourceCommandHandler(
            IMapper mapper,
            INewsSourceRepository newsSourceRepository,
            ILogger<CreateNewsSourceCommandHandler> logger)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _newsSourceRepository = newsSourceRepository ??
                throw new ArgumentNullException(nameof(newsSourceRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(CreateNewsSourceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var authenticity = request.NewsSourceClassification;

                var validatedNewsLink = ValidateNewsLink(request.NewsLink);

                await ValidateRequestAsync(validatedNewsLink, authenticity);

                await _newsSourceRepository.AddNewsSourceAsync(validatedNewsLink, authenticity);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a news source", request.NewsLink, request.NewsSourceClassification);
                throw;
            }
        }

        private static string ValidateNewsLink(string newsLink)
        {
            var resolver = new DomainValidator();
            return resolver.ValidateDomain(newsLink);
        }

        private async Task ValidateRequestAsync(string newsLink, NewsSourceAuthenticity authenticity)
        {
            var existingSource = await _newsSourceRepository.SearchNewsSourceAsync(newsLink);
            if (existingSource is null)
                return;

            existingSource.UpdateAuthenticity(authenticity);
        }
    }
}

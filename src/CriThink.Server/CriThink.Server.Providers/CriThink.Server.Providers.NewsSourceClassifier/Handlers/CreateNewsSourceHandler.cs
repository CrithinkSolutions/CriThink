using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Infrastructure.Repositories;
using MediatR;

namespace CriThink.Server.Providers.NewsSourceClassifier.Handlers
{
    public class CreateNewsSourceHandler : IRequestHandler<CreateNewsSourceCommand>
    {
        private readonly INewsSourceRepository _newsSourceRepository;

        public CreateNewsSourceHandler(INewsSourceRepository newsSourceRepository)
        {
            _newsSourceRepository = newsSourceRepository ?? throw new ArgumentNullException(nameof(newsSourceRepository));
        }

        public async Task<Unit> Handle(CreateNewsSourceCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.IsGoodSource)
                await _newsSourceRepository.AddNewsSourceToWhitelist(request.Uri, request.Authencity);
            else
                await _newsSourceRepository.AddNewsSourceToBlacklist(request.Uri, request.Authencity);

            return Unit.Value;
        }
    }
}

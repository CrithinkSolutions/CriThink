using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Infrastructure.Repositories;
using MediatR;
// ReSharper disable UnusedMember.Global

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class RemoveBadNewsSourceHandler : IRequestHandler<RemoveBadNewsSourceCommand>
    {
        private readonly INewsSourceRepository _newsSourceRepository;

        public RemoveBadNewsSourceHandler(INewsSourceRepository newsSourceRepository)
        {
            _newsSourceRepository = newsSourceRepository ?? throw new ArgumentNullException(nameof(newsSourceRepository));
        }

        public async Task<Unit> Handle(RemoveBadNewsSourceCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            await _newsSourceRepository.RemoveNewsSourceFromBlacklistAsync(request.Uri).ConfigureAwait(false);
            return Unit.Value;
        }
    }

    internal class RemoveGoodNewsSourceHandler : IRequestHandler<RemoveGoodNewsSourceCommand>
    {
        private readonly INewsSourceRepository _newsSourceRepository;

        public RemoveGoodNewsSourceHandler(INewsSourceRepository newsSourceRepository)
        {
            _newsSourceRepository = newsSourceRepository ?? throw new ArgumentNullException(nameof(newsSourceRepository));
        }

        public async Task<Unit> Handle(RemoveGoodNewsSourceCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            await _newsSourceRepository.RemoveNewsSourceFromWhitelistAsync(request.Uri).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}

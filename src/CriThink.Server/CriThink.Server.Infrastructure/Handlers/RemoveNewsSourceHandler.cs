using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Infrastructure.Repositories;
using MediatR;
// ReSharper disable UnusedMember.Global

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class RemoveBadNewsSourceHandler : IRequestHandler<RemoveNewsSourceCommand>
    {
        private readonly INewsSourceRepository _newsSourceRepository;

        public RemoveBadNewsSourceHandler(INewsSourceRepository newsSourceRepository)
        {
            _newsSourceRepository = newsSourceRepository ?? throw new ArgumentNullException(nameof(newsSourceRepository));
        }

        public async Task<Unit> Handle(RemoveNewsSourceCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            await _newsSourceRepository.RemoveNewsSourceAsync(request.NewsLink).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}

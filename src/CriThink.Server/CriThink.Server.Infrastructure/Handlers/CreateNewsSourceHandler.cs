using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Infrastructure.Repositories;
using MediatR;

namespace CriThink.Server.Infrastructure.Handlers
{
    // ReSharper disable once UnusedMember.Global
    internal class CreateNewsSourceHandler : IRequestHandler<CreateNewsSourceCommand>
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

            await _newsSourceRepository.AddNewsSourceAsync(request.Uri, request.Authencity).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}

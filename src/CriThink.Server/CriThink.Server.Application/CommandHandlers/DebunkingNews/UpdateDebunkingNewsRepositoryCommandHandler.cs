using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Facades;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class UpdateDebunkingNewsRepositoryCommandHandler : IRequestHandler<UpdateDebunkingNewsRepositoryCommand>
    {
        private readonly IDebunkNewsFetcherFacade _debunkNewsFetcherFacade;
        private readonly IDebunkingNewsRepository _debunkingNewsRepository;
        private readonly IDebunkingNewsTriggerLogRepository _debunkingNewsTriggerLogRepository;
        private readonly ILogger<UpdateDebunkingNewsRepositoryCommandHandler> _logger;

        public UpdateDebunkingNewsRepositoryCommandHandler(
            IDebunkingNewsTriggerLogRepository debunkingNewsTriggerLogRepository,
            IDebunkNewsFetcherFacade debunkNewsFetcherFacade,
            IDebunkingNewsRepository debunkingNewsRepository,
            ILogger<UpdateDebunkingNewsRepositoryCommandHandler> logger)
        {
            _debunkingNewsTriggerLogRepository = debunkingNewsTriggerLogRepository ??
                throw new ArgumentNullException(nameof(debunkingNewsTriggerLogRepository));

            _debunkNewsFetcherFacade = debunkNewsFetcherFacade ??
                throw new ArgumentNullException(nameof(debunkNewsFetcherFacade));

            _debunkingNewsRepository = debunkingNewsRepository ??
                throw new ArgumentNullException(nameof(debunkingNewsRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateDebunkingNewsRepositoryCommand request, CancellationToken cancellationToken)
        {
            DebunkingNewsTriggerLog triggerLog = null;

            try
            {
                var lastFetchingTimeStamp = await _debunkingNewsTriggerLogRepository.GetLatestTimeStampAsync();
                var debunkingNewsCollection = await _debunkNewsFetcherFacade.FetchDebunkingNewsAsync(lastFetchingTimeStamp);

                if (!debunkingNewsCollection.Any())
                    return Unit.Value;

                foreach (var dNews in debunkingNewsCollection.SelectMany(x => x.DebunkingNewsCollection))
                {
                    await _debunkingNewsRepository.AddDebunkingNewsAsync(dNews);
                }

                triggerLog = DebunkingNewsTriggerLog.Create();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error to fetch debunking news");
                triggerLog = DebunkingNewsTriggerLog.Create(ex.Message);
                throw;
            }
            finally
            {
                await _debunkingNewsTriggerLogRepository.AddTriggerLogAsync(triggerLog);
            }

            return Unit.Value;
        }
    }
}

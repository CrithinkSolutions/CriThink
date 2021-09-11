using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Exceptions;
using CriThink.Server.Application.Facades;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Providers.DebunkingNewsFetcher;
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
            DebunkingNewsProviderResult[] debunkingNewsCollection;
            DebunkingNewsTriggerLog triggerLog = null;
            StringBuilder errorLog = null;

            try
            {
                try
                {
                    debunkingNewsCollection = await FetchDebunkingNewsAsync();
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error to fetch debunking news");
                    triggerLog = DebunkingNewsTriggerLog.CreateWithFailure(
                        $"{nameof(UpdateDebunkingNewsRepositoryCommandHandler)}: {ex.Message}");

                    throw;
                }

                if (!debunkingNewsCollection.Any())
                {
                    triggerLog = DebunkingNewsTriggerLog.CreateWithSuccess();
                    return Unit.Value;
                }

                foreach (var dNews in debunkingNewsCollection.SelectMany(x => x.DebunkingNewsCollection))
                {
                    if (dNews.HasErrror)
                    {
                        errorLog ??= new StringBuilder();
                        errorLog.AppendLine(dNews.Exception.Message);
                        continue;
                    }

                    try
                    {
                        await _debunkingNewsRepository.AddDebunkingNewsAsync(dNews.Result);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Error to save debunking news");
                        triggerLog = DebunkingNewsTriggerLog.CreateWithFailure(
                            $"{nameof(UpdateDebunkingNewsRepositoryCommandHandler)}: {ex.Message}");

                        throw;
                    }
                }

                triggerLog = GetLogByAnyError(errorLog);
            }
            catch (DebunkingNewsFetcherPartialFailureException)
            {
                triggerLog = DebunkingNewsTriggerLog.CreateWithPartialFailure(errorLog.ToString());
                throw;
            }
            finally
            {
                await _debunkingNewsTriggerLogRepository.AddTriggerLogAsync(triggerLog);
                await _debunkingNewsTriggerLogRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }

            return Unit.Value;
        }

        private async Task<DebunkingNewsProviderResult[]> FetchDebunkingNewsAsync()
        {
            var lastFetchingTimeStamp = await _debunkingNewsTriggerLogRepository.GetLatestTimeStampAsync();
            var debunkingNewsCollection = await _debunkNewsFetcherFacade.FetchDebunkingNewsAsync(lastFetchingTimeStamp);

            return debunkingNewsCollection;
        }

        private static DebunkingNewsTriggerLog GetLogByAnyError(StringBuilder errorLog)
        {
            if (errorLog.Length <= 0)
                return DebunkingNewsTriggerLog.CreateWithSuccess();

            throw new DebunkingNewsFetcherPartialFailureException();
        }
    }
}

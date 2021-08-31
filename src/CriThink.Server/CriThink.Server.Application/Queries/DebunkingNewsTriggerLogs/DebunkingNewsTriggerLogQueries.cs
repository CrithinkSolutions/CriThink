using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Core.QueryResults;
using CriThink.Server.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.Queries
{
    internal class DebunkingNewsTriggerLogQueries : IDebunkingNewsTriggerLogQueries
    {
        private readonly IMapper _mapper;
        private readonly IDebunkingNewsTriggerLogRepository _debunkingNewsTriggerLogRepository;
        private readonly ILogger<DebunkingNewsTriggerLogQueries> _logger;

        public DebunkingNewsTriggerLogQueries(
            IMapper mapper,
            IDebunkingNewsTriggerLogRepository debunkingNewsTriggerLogRepository,
            ILogger<DebunkingNewsTriggerLogQueries> logger)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _debunkingNewsTriggerLogRepository = debunkingNewsTriggerLogRepository ??
                throw new ArgumentNullException(nameof(debunkingNewsTriggerLogRepository));

            _logger = logger;
        }

        public async Task<DateTime> GetLastDebunkinNewsFetchTimestampAsync()
        {
            _logger?.LogInformation(nameof(GetLastDebunkinNewsFetchTimestampAsync));

            var latestTriggerDateTime = await _debunkingNewsTriggerLogRepository.GetLatestTimeStampAsync();

            _logger?.LogInformation($"{nameof(GetLastDebunkinNewsFetchTimestampAsync)}: done");

            return latestTriggerDateTime;
        }

        public async Task<TriggerLogsGetAllResponse> GetAllTriggerLogsAsync(int pageSize, int pageIndex)
        {
            _logger?.LogInformation(nameof(GetAllTriggerLogsAsync));

            var triggerLogs = await _debunkingNewsTriggerLogRepository.GetAllTriggerLogsAsync(
                pageSize,
                pageIndex);

            var dtos = triggerLogs
                .Take(pageSize)
                .Select(triggerLog => _mapper.Map<GetAllTriggerLogQueryResult, TriggerLogGetResponse>(triggerLog))
                .ToList();

            var response = new TriggerLogsGetAllResponse(dtos, triggerLogs.Count > pageSize);

            _logger?.LogInformation($"{nameof(GetAllTriggerLogsAsync)}: done");

            return response;
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Server.Application.Administration.ViewModels;
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

        public async Task<TriggerLogsGetAllViewModel> GetAllTriggerLogsAsync(int pageSize, int pageIndex)
        {
            _logger?.LogInformation(nameof(GetAllTriggerLogsAsync));

            var triggerLogs = await _debunkingNewsTriggerLogRepository.GetAllTriggerLogsAsync(
                pageSize,
                pageIndex);

            var dtos = triggerLogs
                .Take(pageSize)
                .Select(triggerLog => _mapper.Map<GetAllTriggerLogQueryResult, TriggerLogGetViewModel>(triggerLog))
                .ToList();

            var response = new TriggerLogsGetAllViewModel(dtos, triggerLogs.Count > pageSize);

            _logger?.LogInformation($"{nameof(GetAllTriggerLogsAsync)}: done");

            return response;
        }
    }
}

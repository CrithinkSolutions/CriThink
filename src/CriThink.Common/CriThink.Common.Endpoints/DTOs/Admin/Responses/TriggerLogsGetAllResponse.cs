
using System.Collections.Generic;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class TriggerLogsGetAllResponse
    {
        public TriggerLogsGetAllResponse() { }

        public TriggerLogsGetAllResponse(IEnumerable<TriggerLogGetResponse> logsCollection, bool hasNextPage)
        {
            LogsCollection = new List<TriggerLogGetResponse>(logsCollection).AsReadOnly();
            HasNextPage = hasNextPage;
        }

        [JsonPropertyName("logCollection")]
        public IReadOnlyCollection<TriggerLogGetResponse> LogsCollection { get; set; }

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }
    }
}

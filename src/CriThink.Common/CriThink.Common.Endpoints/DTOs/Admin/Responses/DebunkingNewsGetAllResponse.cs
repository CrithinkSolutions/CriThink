using System.Collections.Generic;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class DebunkingNewsGetAllResponse
    {
        public DebunkingNewsGetAllResponse(IEnumerable<DebunkingNewsGetResponse> debunkingNewsCollection, bool hasNextPage)
        {
            DebunkingNewsCollection = new List<DebunkingNewsGetResponse>(debunkingNewsCollection).AsReadOnly();
            HasNextPage = hasNextPage;
        }

        [JsonPropertyName("debunkingNews")]
        public IReadOnlyCollection<DebunkingNewsGetResponse> DebunkingNewsCollection { get; }

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; }
    }
}

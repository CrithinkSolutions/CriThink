using System.Collections.Generic;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class UnknownNewsSourceGetAllResponse
    {
        public UnknownNewsSourceGetAllResponse() { }

        public UnknownNewsSourceGetAllResponse(IEnumerable<UnknownNewsSourceGetResponse> collection, bool hasNextPage)
        {
            UnknownNewsSourceCollection = new List<UnknownNewsSourceGetResponse>(collection).AsReadOnly();
            HasNextPage = hasNextPage;
        }

        [JsonPropertyName("unknownNewsSourceCollection")]
        public IReadOnlyCollection<UnknownNewsSourceGetResponse> UnknownNewsSourceCollection { get; set; }

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }
    }
}
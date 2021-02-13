using System.Collections.Generic;
using System.Text.Json.Serialization;

// ReSharper disable CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceGetAllResponse
    {
        public NewsSourceGetAllResponse() { }

        public NewsSourceGetAllResponse(IEnumerable<NewsSourceGetResponse> newsSourceCollection, bool hasNextPage)
        {
            NewsSourcesCollection = new List<NewsSourceGetResponse>(newsSourceCollection).AsReadOnly();
            HasNextPage = hasNextPage;
        }

        [JsonPropertyName("newsSources")]
        public IReadOnlyCollection<NewsSourceGetResponse> NewsSourcesCollection { get; set; }

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourcePostAllAnswersRequest
    {
        [JsonPropertyName("questions")]
        public IList<NewsSourcePostAnswersRequest> Questions { get; set; }

        [JsonPropertyName("newsLink")]
        public string NewsLink { get; set; }
    }
}

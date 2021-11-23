using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceSearchByTextResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("favIcon")]
        public string FavIcon { get; set; }

        [JsonPropertyName("authenticity")]
        public string Authenticity { get; set; }
    }
}

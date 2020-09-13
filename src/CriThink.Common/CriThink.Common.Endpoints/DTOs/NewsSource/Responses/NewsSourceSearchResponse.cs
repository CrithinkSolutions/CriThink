﻿using System.Text.Json.Serialization;

// Resharper disable CheckNamespace

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceSearchResponse
    {
        [JsonPropertyName("classification")]
        [JsonConverter(typeof(NewsSourceClassificationConverter))]
        public NewsSourceClassification Classification { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}

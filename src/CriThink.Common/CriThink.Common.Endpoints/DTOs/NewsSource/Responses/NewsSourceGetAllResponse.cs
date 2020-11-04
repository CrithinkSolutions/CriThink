﻿using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.Converters;

// ReSharper disable CheckNamespace
#pragma warning disable CA1056 // Uri properties should not be strings

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceGetAllResponse
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("classification")]
        [JsonConverter(typeof(NewsSourceClassificationConverter))]
        public NewsSourceClassification NewsSourceClassification { get; set; }
    }
}

﻿using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.DebunkingNews
{
    public class DebunkingNewsGetResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("publisher")]
        public string Publisher { get; set; }

        [JsonPropertyName("publisherLanguage")]
        public string PublisherLanguage { get; set; }

        [JsonPropertyName("publisherLanguageCode")]
        public string PublisherLanguageCode { get; set; }

        [JsonPropertyName("publisherCountry")]
        public string PublisherCountry { get; set; }

        [JsonPropertyName("publisherCountryCode")]
        public string PublisherCountryCode { get; set; }

        [JsonPropertyName("newsLink")]
        public string NewsLink { get; set; }

        [JsonPropertyName("newsImageLink")]
        public string NewsImageLink { get; set; }

        [JsonPropertyName("newsCaption")]
        public string NewsCaption { get; set; }
    }
}
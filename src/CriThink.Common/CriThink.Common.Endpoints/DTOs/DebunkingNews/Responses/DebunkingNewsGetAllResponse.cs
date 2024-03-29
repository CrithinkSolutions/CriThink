﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.DebunkingNews
{
    public class DebunkingNewsGetAllResponse
    {
        public DebunkingNewsGetAllResponse() { }

        public DebunkingNewsGetAllResponse(IEnumerable<DebunkingNewsGetResponse> debunkingNewsCollection, bool hasNextPage)
        {
            DebunkingNewsCollection = new List<DebunkingNewsGetResponse>(debunkingNewsCollection).AsReadOnly();
            HasNextPage = hasNextPage;
        }

        [JsonPropertyName("debunkingNews")]
        public IReadOnlyCollection<DebunkingNewsGetResponse> DebunkingNewsCollection { get; set; }

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }
    }
}

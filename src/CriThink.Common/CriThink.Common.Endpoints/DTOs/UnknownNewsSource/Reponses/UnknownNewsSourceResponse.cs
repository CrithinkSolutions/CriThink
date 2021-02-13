using System;
using CriThink.Common.Endpoints.DTOs.NewsSource;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.UnknownNewsSource
{
    public class UnknownNewsSourceResponse
    {
        public Guid Id { get; set; }

        public string Uri { get; set; }

        public NewsSourceClassification Classification { get; set; }
    }
}

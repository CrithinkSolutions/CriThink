using System;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class UnknownNewsSourceResponse
    {
        public Guid Id { get; set; }

        public string Uri { get; set; }

        public NewsSourceClassification Classification { get; set; }
    }
}

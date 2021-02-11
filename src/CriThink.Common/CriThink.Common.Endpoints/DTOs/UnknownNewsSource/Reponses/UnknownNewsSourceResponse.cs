using System;
using CriThink.Common.Endpoints.DTOs.NewsSource;

namespace CriThink.Common.Endpoints.DTOs.UnknownNewsSource
{
    public class UnknownNewsSourceResponse
    {
        public Guid Id { get; set; }
        public string Uri { get; set; }
        public NewsSourceClassification Classification { get; set; }
    }
}

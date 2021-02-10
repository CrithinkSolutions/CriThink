using CriThink.Common.Endpoints.DTOs.NewsSource;

namespace CriThink.Common.Endpoints.DTOs.UnknownNewsSource.Requests
{
    public class TriggerUpdateForIdentifiedNewsSourceRequest
    {
        public string Uri { get; set; }
        public NewsSourceClassification Classification { get; set; }
    }
}

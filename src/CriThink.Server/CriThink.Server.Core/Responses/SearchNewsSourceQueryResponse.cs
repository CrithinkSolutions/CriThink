using CriThink.Server.Core.Commands;

namespace CriThink.Server.Core.Responses
{
    public class SearchNewsSourceQueryResponse
    {
        public SearchNewsSourceQueryResponse(NewsSourceAuthencity sourceAuthencity)
        {
            SourceAuthencity = sourceAuthencity;
        }

        public NewsSourceAuthencity SourceAuthencity { get; }
    }
}

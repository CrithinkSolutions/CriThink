using CriThink.Server.Core.Commands;

namespace CriThink.Server.Core.Responses
{
    public class SearchNewsSourceResponse
    {
        public SearchNewsSourceResponse(NewsSourceAuthencity sourceAuthencity)
        {
            SourceAuthencity = sourceAuthencity;
        }

        public NewsSourceAuthencity SourceAuthencity { get; }
    }
}

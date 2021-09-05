using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.QueryResults
{
    public class GetAllNewsSourceQueryResult
    {
        public GetAllNewsSourceQueryResult(string newsLink, NewsSourceAuthenticity sourceAuthencity)
        {
            NewsLink = newsLink;
            SourceAuthencity = sourceAuthencity;
        }

        public string NewsLink { get; }

        public NewsSourceAuthenticity SourceAuthencity { get; }
    }
}

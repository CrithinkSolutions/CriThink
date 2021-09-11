using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Domain.QueryResults
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

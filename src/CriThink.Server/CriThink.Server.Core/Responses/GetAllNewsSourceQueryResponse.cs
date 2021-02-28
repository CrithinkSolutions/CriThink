using CriThink.Server.Core.Commands;

namespace CriThink.Server.Core.Responses
{
    public class GetAllNewsSourceQueryResponse
    {
        public GetAllNewsSourceQueryResponse(string newsLink, NewsSourceAuthenticity sourceAuthencity)
        {
            NewsLink = newsLink;
            SourceAuthencity = sourceAuthencity;
        }

        public string NewsLink { get; }

        public NewsSourceAuthenticity SourceAuthencity { get; }
    }
}

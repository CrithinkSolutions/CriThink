using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Domain.QueryResults
{
    public class SearchNewsSourceQueryResponse
    {
        public SearchNewsSourceQueryResponse(NewsSourceAuthenticity sourceAuthenticity, string description)
        {
            SourceAuthenticity = sourceAuthenticity;
            Description = description;
        }

        public NewsSourceAuthenticity SourceAuthenticity { get; }

        public string Description { get; }
    }
}

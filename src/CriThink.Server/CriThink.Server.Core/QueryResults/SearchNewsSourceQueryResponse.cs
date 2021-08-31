using CriThink.Server.Core.Commands;

namespace CriThink.Server.Core.QueryResults
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

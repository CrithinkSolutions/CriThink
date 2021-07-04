namespace CriThink.Server.Core.Responses
{
    public class GetStatisticsSearchesCountingQueryResponse
    {
        public GetStatisticsSearchesCountingQueryResponse(long searchesCounting)
        {
            SearchesCounting = searchesCounting;
        }

        public long SearchesCounting { get; }
    }
}

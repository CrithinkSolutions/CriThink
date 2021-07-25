namespace CriThink.Server.Core.Responses
{
    public class GetStatisticsArticleRatesCountingQueryResponse
    {
        public GetStatisticsArticleRatesCountingQueryResponse(long ratesCounting)
        {
            RatesCounting = ratesCounting;
        }

        public long RatesCounting { get; }
    }
}

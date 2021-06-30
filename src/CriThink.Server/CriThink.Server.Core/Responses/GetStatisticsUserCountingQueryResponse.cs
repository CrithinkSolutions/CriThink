namespace CriThink.Server.Core.Responses
{
    public class GetStatisticsUserCountingQueryResponse
    {
        public GetStatisticsUserCountingQueryResponse(long usersCounting)
        {
            UsersCounting = usersCounting;
        }

        public long UsersCounting { get; }
    }
}

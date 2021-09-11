namespace CriThink.Client.Core.Models.Statistics
{
    public class StatisticsDetailModel
    {
        public StatisticsDetailModel(long platformUsers, long platformSearches, long userSearches)
        {
            PlatformUsers = platformUsers;
            PlatformSearches = platformSearches;
            UserSearches = userSearches;
        }

        public long PlatformUsers { get; }

        public long PlatformSearches { get; }

        public long UserSearches { get; }
    }
}

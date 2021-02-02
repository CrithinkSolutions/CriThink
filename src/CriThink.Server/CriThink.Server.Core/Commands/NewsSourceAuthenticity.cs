namespace CriThink.Server.Core.Commands
{
    /// <summary>
    /// Represents the news source categories
    /// </summary>
    public enum NewsSourceAuthenticity
    {
        // Good
        Reliable = 0,
        Satirical = 1,

        // Bad
        Conspiracist = 10,
        FakeNews = 11,

        // Not yet analized sources
        Unknown = 9999,
    }
}

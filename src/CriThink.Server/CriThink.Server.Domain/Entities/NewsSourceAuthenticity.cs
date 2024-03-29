﻿namespace CriThink.Server.Domain.Entities
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

        // So and so
        Suspicious = 12,
        SocialMedia = 13,

        // Not yet analized sources
        Unknown = 9999,
    }
}

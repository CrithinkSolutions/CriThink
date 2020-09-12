﻿namespace CriThink.Server.Core.Commands
{
    /// <summary>
    /// Represents the news source categories
    /// </summary>
    public enum NewsSourceAuthenticity
    {
        // Good
        Trusted = 0,
        Satiric = 1,

        // Bad
        Cospiracy = 10,
        Fake = 11
    }
}
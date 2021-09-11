// ReSharper disable once CheckNamespace

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public enum NewsSourceAuthenticityDto
    {
        /// <summary>
        /// Represents a trusted news source
        /// </summary>
        Reliable,

        /// <summary>
        /// Represents a well-known satiric news source
        /// </summary>
        Satirical,

        /// <summary>
        /// Represents a cospiracy news source.
        /// </summary>
        Conspiracist,

        /// <summary>
        /// Represents a fake news source
        /// </summary>
        FakeNews,

        /// <summary>
        /// Represents a suspicious source
        /// </summary>
        Suspicious,

        /// <summary>
        /// Represents a social network
        /// </summary>
        SocialMedia,

        /// <summary>
        /// Represents a site that isn't already identified
        /// </summary>
        Unknown,
    }
}

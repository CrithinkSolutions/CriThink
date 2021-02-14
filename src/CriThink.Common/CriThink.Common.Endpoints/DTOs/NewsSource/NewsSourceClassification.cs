// ReSharper disable once CheckNamespace

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public enum NewsSourceClassification
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
        /// Represents a site that isn't already identified
        /// </summary>
        Unknown,
    }
}

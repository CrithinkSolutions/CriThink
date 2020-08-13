// ReSharper disable once CheckNamespace

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public enum NewsSourceClassification
    {
        /// <summary>
        /// Represents the best rate for a news source. Dedicated to famous websites
        /// </summary>
        Authentic,

        /// <summary>
        /// Represents a non famous news source but still trusty
        /// </summary>
        Secure,

        /// <summary>
        /// Represents a news source not fully trusted. User must be carefoul
        /// </summary>
        NotTrusted,

        /// <summary>
        /// Represents a fake news source
        /// </summary>
        Fake
    }
}

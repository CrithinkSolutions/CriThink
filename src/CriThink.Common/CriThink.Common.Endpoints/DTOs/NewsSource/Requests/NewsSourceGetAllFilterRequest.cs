﻿// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    /// <summary>
    /// Options to filter news sources
    /// </summary>
    public enum NewsSourceGetAllFilterRequest
    {
        /// <summary>
        /// No filter applied
        /// </summary>
        None,

        /// <summary>
        /// Get only white listed news sources
        /// </summary>
        Good,

        /// <summary>
        /// Get only black listed news sources
        /// </summary>
        Bad
    }
}

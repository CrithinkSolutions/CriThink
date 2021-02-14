using System;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    [Flags]
    public enum DebunkingNewsGetAllLanguageFilterRequests
    {
        /// <summary>
        /// No filter applied
        /// </summary>
        None = 0,

        English = 1,

        Italian = 2,
    }
}

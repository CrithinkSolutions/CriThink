using System;
using System.Collections.Generic;
using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    /// <summary>
    /// Creates the query to get all debunking news paginated. Returns one item more than
    /// required to facilitate paging for the client
    /// </summary>
    public class GetAllDebunkingNewsQuery : IRequest<IList<GetAllDebunkingNewsQueryResponse>>
    {
        public GetAllDebunkingNewsQuery(int size, int index, GetAllDebunkingNewsLanguageFilters languageFilters)
        {
            Size = size;
            Index = index;
            LanguageFilters = languageFilters;
        }

        public int Size { get; }

        public int Index { get; }

        public GetAllDebunkingNewsLanguageFilters LanguageFilters { get; }
    }

    [Flags]
    public enum GetAllDebunkingNewsLanguageFilters
    {
        /// <summary>
        /// All languages
        /// </summary>
        All,

        /// <summary>
        /// English language
        /// </summary>
        English,

        /// <summary>
        /// Italian language
        /// </summary>
        Italian,
    }
}

using System;
using System.Collections.Generic;
using CriThink.Common.Helpers;
using CriThink.Server.Core.Entities;
using CriThink.Server.Providers.Common;

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Providers.DebunkingNewsFetcher
{
    public class DebunkingNewsProviderResult : IProviderResult
    {
        public DebunkingNewsProviderResult(IEnumerable<Monad<DebunkingNews>> responses)
        {
            if (responses is null)
                throw new ArgumentNullException(nameof(responses));

            DebunkingNewsCollection = new List<Monad<DebunkingNews>>(responses).AsReadOnly();
        }

        public DebunkingNewsProviderResult(Exception ex, string errorDescription = null)
        {
            Exception = ex ?? throw new ArgumentNullException(nameof(ex));
            ErrorDescription = string.IsNullOrWhiteSpace(errorDescription) ? ex.Message : errorDescription;
        }

        public Exception Exception { get; }

        public bool HasError => Exception != null;

        public string ErrorDescription { get; }

        public IReadOnlyList<Monad<DebunkingNews>> DebunkingNewsCollection { get; }
    }
}

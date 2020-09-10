using System;
using System.Collections.Generic;
using CriThink.Server.Core.Providers;

// ReSharper disable once CheckNamespace

namespace CriThink.Server.Providers.DebunkNewsFetcher
{
    public class DebunkNewsProviderResult : IProviderResult
    {
        public DebunkNewsProviderResult(IEnumerable<DebunkNewsResponse> responses)
        {
            if (responses == null)
                throw new ArgumentNullException(nameof(responses));

            Responses = new List<DebunkNewsResponse>(responses).AsReadOnly();
        }

        public DebunkNewsProviderResult(Exception ex, string errorDescription = "")
        {
            Exception = ex ?? throw new ArgumentNullException(nameof(ex));
            ErrorDescription = string.IsNullOrWhiteSpace(errorDescription) ? ex.Message : errorDescription;
        }

        public Exception Exception { get; }

        public bool HasError => Exception != null;

        public string ErrorDescription { get; }

        public IReadOnlyList<DebunkNewsResponse> Responses { get; }
    }
}

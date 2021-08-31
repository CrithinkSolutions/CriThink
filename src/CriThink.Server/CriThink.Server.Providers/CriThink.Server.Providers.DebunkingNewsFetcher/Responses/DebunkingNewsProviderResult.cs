﻿using System;
using System.Collections.Generic;
using CriThink.Server.Core.Entities;
using CriThink.Server.Providers.Common;

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Providers.DebunkingNewsFetcher
{
    public class DebunkingNewsProviderResult : IProviderResult
    {
        public DebunkingNewsProviderResult(IEnumerable<DebunkingNews> responses)
        {
            if (responses is null)
                throw new ArgumentNullException(nameof(responses));

            DebunkingNewsCollection = new List<DebunkingNews>(responses).AsReadOnly();
        }

        public DebunkingNewsProviderResult(Exception ex, string errorDescription = "")
        {
            Exception = ex ?? throw new ArgumentNullException(nameof(ex));
            ErrorDescription = string.IsNullOrWhiteSpace(errorDescription) ? ex.Message : errorDescription;
        }

        public Exception Exception { get; }

        public bool HasError => Exception != null;

        public string ErrorDescription { get; }

        public IReadOnlyList<DebunkingNews> DebunkingNewsCollection { get; }
    }
}

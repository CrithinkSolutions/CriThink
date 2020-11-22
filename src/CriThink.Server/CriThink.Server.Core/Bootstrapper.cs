﻿using CriThink.Server.Core.Facades;
using CriThink.Server.Core.Identity;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Core
{
    public static class Bootstrapper
    {
        public static void AddCore(this IServiceCollection serviceCollection)
        {
            // Facades
            serviceCollection.AddTransient<IDebunkNewsFetcherFacade, DebunkNewsFetcherFacade>();
            serviceCollection.AddTransient<IDomainAnalyzerFacade, DomainAnalyzerFacade>();
            serviceCollection.AddTransient<INewsAnalyzerFacade, NewsAnalyzerFacade>();

            // Services
            serviceCollection.AddTransient<IIdentityService, IdentityService>();
            serviceCollection.AddTransient<INewsAnalyzerService, NewsAnalyzerService>();
            serviceCollection.AddTransient<IDebunkingNewsService, DebunkingNewsService>();
            serviceCollection.AddTransient<INewsSourceService, NewsSourceService>();
        }
    }
}
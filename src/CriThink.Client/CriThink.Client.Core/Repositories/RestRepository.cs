﻿using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using MvvmCross.Logging;

namespace CriThink.Client.Core.Repositories
{
    public class RestRepository : IRestRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMvxLog _logger;

        public RestRepository(IHttpClientFactory httpClientFactory, IMvxLogProvider mvxLogProvider)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = mvxLogProvider?.GetLogFor<RestRepository>();
        }

        public Task MakeRequestAsync(string request, HttpVerb verb, CancellationToken cancellationToken = default) =>
            MakeRestRequestAsync(request, verb, string.Empty, default, default, cancellationToken);

        public Task MakeRequestAsync(string request, HttpVerb verb, object data, CancellationToken cancellationToken = default) =>
            MakeRestRequestAsync(request, verb, string.Empty, data, default, cancellationToken);

        public Task<T> MakeRequestAsync<T>(string request, HttpVerb verb, CancellationToken cancellationToken = default) =>
            MakeRestRequestAsync<T>(request, verb, string.Empty, default, default, cancellationToken);

        public Task<T> MakeRequestAsync<T>(string request, HttpVerb verb, string httpClientName, CancellationToken cancellationToken = default) =>
            MakeRestRequestAsync<T>(request, verb, httpClientName, default, default, cancellationToken);

        public Task<T> MakeRequestAsync<T>(string request, HttpVerb verb, string httpClientName, string apiVersion, CancellationToken cancellationToken = default) =>
            MakeRestRequestAsync<T>(request, verb, httpClientName, default, apiVersion, cancellationToken);

        public Task<T> MakeRequestAsync<T>(string request, HttpVerb verb, object data, CancellationToken cancellationToken = default)
        {
            if ((int) verb < 4)
                throw new InvalidOperationException($"Can't use {nameof(HttpVerb.Get)} with data");

            return MakeRestRequestAsync<T>(request, verb, string.Empty, data, default, cancellationToken);
        }

        public Task<T> MakeRequestAsync<T>(string request, HttpVerb verb, object data, string apiVersion, CancellationToken cancellationToken = default)
        {
            if ((int) verb < 4)
                throw new InvalidOperationException($"Can't use {nameof(HttpVerb.Get)} with data");

            return MakeRestRequestAsync<T>(request, verb, string.Empty, data, apiVersion, cancellationToken);
        }

        public Task<T> MakeRequestAsync<T>(string request, HttpVerb verb, string httpClientName, object data, CancellationToken cancellationToken = default)
        {
            if ((int) verb < 4)
                throw new InvalidOperationException($"Can't use {nameof(HttpVerb.Get)} with data");

            return MakeRestRequestAsync<T>(request, verb, httpClientName, data, default, cancellationToken);
        }

        public Task<T> MakeRequestAsync<T>(string request, HttpVerb verb, string httpClientName, object data, string apiVersion, CancellationToken cancellationToken = default)
        {
            if ((int) verb < 4)
                throw new InvalidOperationException($"Can't use {nameof(HttpVerb.Get)} with data");

            return MakeRestRequestAsync<T>(request, verb, httpClientName, data, apiVersion, cancellationToken);
        }

        private Task MakeRestRequestAsync(string request, HttpVerb verb, string httpClientName, object data, string apiVersion, CancellationToken cancellationToken)
        {
            var httpClient = ResolveHttpClient(httpClientName);
            AddApiVersion(httpClient, apiVersion);

            var uri = GetUri(request);

            return verb switch
            {
                HttpVerb.Get => MakeGetRequestAsync(httpClient, uri, cancellationToken),
                HttpVerb.Post => MakePostRequestAsync(httpClient, uri, data, cancellationToken),
                HttpVerb.Put => MakePutRequestAsync(httpClient, uri, data, cancellationToken),
                HttpVerb.Patch => MakePatchRequestAsync(httpClient, uri, data, cancellationToken),
                HttpVerb.Delete => MakeDeleteRequestAsync(httpClient, uri, cancellationToken),
                _ => throw new ArgumentOutOfRangeException(nameof(verb), $"Unknown {nameof(HttpVerb)} type")
            };
        }

        private Task<T> MakeRestRequestAsync<T>(string request, HttpVerb verb, string httpClientName, object data, string apiVersion, CancellationToken cancellationToken)
        {
            var httpClient = ResolveHttpClient(httpClientName);
            AddApiVersion(httpClient, apiVersion);

            var uri = GetUri(request);

            return verb switch
            {
                HttpVerb.Get => MakeGetRequestAsync<T>(httpClient, uri, cancellationToken),
                HttpVerb.Post => MakePostRequestAsync<T>(httpClient, uri, data, cancellationToken),
                HttpVerb.Put => MakePutRequestAsync<T>(httpClient, uri, data, cancellationToken),
                HttpVerb.Patch => MakePatchRequestAsync<T>(httpClient, uri, data, cancellationToken),
                HttpVerb.Delete => MakeDeleteRequestAsync<T>(httpClient, uri, cancellationToken),
                _ => throw new ArgumentOutOfRangeException(nameof(verb), $"Unknown {nameof(HttpVerb)} type")
            };
        }

        private async Task MakeGetRequestAsync(HttpClient httpClient, Uri uri, CancellationToken cancellationToken)
        {
            try
            {
                var response = await httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger?.Log(MvxLogLevel.Fatal, () => $"Error GET the uri '{uri}", ex);
                throw;
            }
        }

        private async Task<T> MakeGetRequestAsync<T>(HttpClient httpClient, Uri uri, CancellationToken cancellationToken)
        {
            try
            {
                var response = await httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
                return await ProcessHttpRequest<T>(response, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.Log(MvxLogLevel.Fatal, () => $"Error GET the uri '{uri}", ex);
                throw;
            }
        }

        private async Task MakePostRequestAsync(HttpClient httpClient, Uri uri, object data, CancellationToken cancellationToken)
        {
            try
            {
                var serializedData = JsonSerializer.Serialize(data);
                using var httpContent = new StringContent(serializedData, Encoding.UTF8, "application/json");
                using var response = await httpClient.PostAsync(uri, httpContent, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger?.Log(MvxLogLevel.Fatal, () => $"Error POST the uri '{uri}", ex);
                throw;
            }
        }

        private async Task<T> MakePostRequestAsync<T>(HttpClient httpClient, Uri uri, object data, CancellationToken cancellationToken)
        {
            try
            {
                var serializedData = JsonSerializer.Serialize(data);
                using var httpContent = new StringContent(serializedData, Encoding.UTF8, "application/json");
                using var response = await httpClient.PostAsync(uri, httpContent, cancellationToken).ConfigureAwait(false);
                return await ProcessHttpRequest<T>(response, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.Log(MvxLogLevel.Fatal, () => $"Error POST the uri '{uri}", ex);
                throw;
            }
        }

        private async Task MakePutRequestAsync(HttpClient httpClient, Uri uri, object data, CancellationToken cancellationToken)
        {
            try
            {
                var serializedData = JsonSerializer.Serialize(data);
                using var httpContent = new StringContent(serializedData, Encoding.UTF8, "application/json");
                using var response = await httpClient.PutAsync(uri, httpContent, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger?.Log(MvxLogLevel.Fatal, () => $"Error PUT the uri '{uri}", ex);
                throw;
            }
        }

        private async Task<T> MakePutRequestAsync<T>(HttpClient httpClient, Uri uri, object data, CancellationToken cancellationToken)
        {
            try
            {
                var serializedData = JsonSerializer.Serialize(data);
                using var httpContent = new StringContent(serializedData, Encoding.UTF8, "application/json");
                using var response = await httpClient.PutAsync(uri, httpContent, cancellationToken).ConfigureAwait(false);
                return await ProcessHttpRequest<T>(response, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.Log(MvxLogLevel.Fatal, () => $"Error PUT the uri '{uri}", ex);
                throw;
            }
        }

        private async Task MakePatchRequestAsync(HttpClient httpClient, Uri uri, object data, CancellationToken cancellationToken)
        {
            try
            {
                var serializedData = JsonSerializer.Serialize(data);
                using var httpContent = new StringContent(serializedData, Encoding.UTF8, "application/json");
                using var response = await httpClient.PatchAsync(uri, httpContent, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger?.Log(MvxLogLevel.Fatal, () => $"Error PATCH the uri '{uri}", ex);
                throw;
            }
        }

        private async Task<T> MakePatchRequestAsync<T>(HttpClient httpClient, Uri uri, object data, CancellationToken cancellationToken)
        {
            try
            {
                var serializedData = JsonSerializer.Serialize(data);
                using var httpContent = new StringContent(serializedData, Encoding.UTF8, "application/json");
                using var response = await httpClient.PatchAsync(uri, httpContent, cancellationToken).ConfigureAwait(false);
                return await ProcessHttpRequest<T>(response, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.Log(MvxLogLevel.Fatal, () => $"Error PATCH the uri '{uri}", ex);
                throw;
            }
        }

        private async Task MakeDeleteRequestAsync(HttpClient httpClient, Uri uri, CancellationToken cancellationToken)
        {
            try
            {
                using var response = await httpClient.DeleteAsync(uri, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger?.Log(MvxLogLevel.Fatal, () => $"Error DELETE the uri '{uri}", ex);
                throw;
            }
        }

        private async Task<T> MakeDeleteRequestAsync<T>(HttpClient httpClient, Uri uri, CancellationToken cancellationToken)
        {
            try
            {
                using var response = await httpClient.DeleteAsync(uri, cancellationToken).ConfigureAwait(false);
                return await ProcessHttpRequest<T>(response, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.Log(MvxLogLevel.Fatal, () => $"Error DELETE the uri '{uri}", ex);
                throw;
            }
        }

        private HttpClient ResolveHttpClient(string httpClientName) => _httpClientFactory.CreateClient(httpClientName);

        private static void AddApiVersion(HttpClient httpClient, string version)
        {
            if (string.IsNullOrWhiteSpace(version)) return;

            httpClient.DefaultRequestHeaders.Remove(EndpointConstants.ApiVersionHeader);
            httpClient.DefaultRequestHeaders.Add(EndpointConstants.ApiVersionHeader, version);
        }

        private static Uri GetUri(string request) => new Uri(request, UriKind.Relative);

        private static Task<T> ProcessHttpRequest<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            response.EnsureSuccessStatusCode();
            return response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        }
    }
}

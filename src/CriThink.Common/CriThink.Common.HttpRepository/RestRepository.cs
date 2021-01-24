using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;

namespace CriThink.Common.HttpRepository
{
    public class RestRepository : IRestRepository
    {
        private const string JsonMediaType = "application/json";

        private readonly IHttpClientFactory _httpClientFactory;

        public RestRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public Task MakeRequestAsync(string request, HttpRestVerb restVerb, string token = null, CancellationToken cancellationToken = default) =>
            MakeRestRequestAsync(request, restVerb, string.Empty, default, default, token, cancellationToken);

        public Task MakeRequestAsync(string request, HttpRestVerb restVerb, string httpClientName, string token = null, CancellationToken cancellationToken = default) =>
            MakeRestRequestAsync(request, restVerb, httpClientName, default, default, token, cancellationToken);

        public Task MakeRequestAsync(string request, HttpRestVerb restVerb, string httpClientName, string apiVersion, string token = null, CancellationToken cancellationToken = default) =>
            MakeRestRequestAsync(request, restVerb, httpClientName, default, apiVersion, token, cancellationToken);

        public Task MakeRequestAsync(string request, HttpRestVerb restVerb, object data, string token = null, CancellationToken cancellationToken = default) =>
            MakeRestRequestAsync(request, restVerb, string.Empty, data, default, token, cancellationToken);

        public Task<T> MakeRequestAsync<T>(string request, HttpRestVerb restVerb, string token = null, CancellationToken cancellationToken = default) =>
            MakeRestRequestAsync<T>(request, restVerb, string.Empty, default, default, token, cancellationToken);

        public Task<T> MakeRequestAsync<T>(string request, HttpRestVerb restVerb, string httpClientName, string token = null, CancellationToken cancellationToken = default) =>
            MakeRestRequestAsync<T>(request, restVerb, httpClientName, default, default, token, cancellationToken);

        public Task<T> MakeRequestAsync<T>(string request, HttpRestVerb restVerb, string httpClientName, string apiVersion, string token = null, CancellationToken cancellationToken = default) =>
            MakeRestRequestAsync<T>(request, restVerb, httpClientName, default, apiVersion, token, cancellationToken);

        public Task<T> MakeRequestAsync<T>(string request, HttpRestVerb restVerb, object data, string token = null, CancellationToken cancellationToken = default)
        {
            if ((int) restVerb < 4)
                throw new InvalidOperationException($"Can't use {nameof(HttpRestVerb.Get)} with data");

            return MakeRestRequestAsync<T>(request, restVerb, string.Empty, data, default, token, cancellationToken);
        }

        public Task<T> MakeRequestAsync<T>(string request, HttpRestVerb restVerb, object data, string apiVersion, string token = null, CancellationToken cancellationToken = default)
        {
            if ((int) restVerb < 4)
                throw new InvalidOperationException($"Can't use {nameof(HttpRestVerb.Get)} with data");

            return MakeRestRequestAsync<T>(request, restVerb, string.Empty, data, apiVersion, token, cancellationToken);
        }

        public Task<T> MakeRequestAsync<T>(string request, HttpRestVerb restVerb, string httpClientName, object data, string token = null, CancellationToken cancellationToken = default)
        {
            if ((int) restVerb < 4)
                throw new InvalidOperationException($"Can't use {nameof(HttpRestVerb.Get)} with data");

            return MakeRestRequestAsync<T>(request, restVerb, httpClientName, data, default, token, cancellationToken);
        }

        public Task<T> MakeRequestAsync<T>(string request, HttpRestVerb restVerb, string httpClientName, object data, string apiVersion, string token = null, CancellationToken cancellationToken = default)
        {
            if ((int) restVerb < 4)
                throw new InvalidOperationException($"Can't use {nameof(HttpRestVerb.Get)} with data");

            return MakeRestRequestAsync<T>(request, restVerb, httpClientName, data, apiVersion, token, cancellationToken);
        }

        private Task MakeRestRequestAsync(string request, HttpRestVerb restVerb, string httpClientName, object data, string apiVersion, string token, CancellationToken cancellationToken)
        {
            var httpClient = ResolveHttpClient(httpClientName);
            AddApiVersion(httpClient, apiVersion);
            AddBearerToken(httpClient, token);

            var uri = GetUri(request);

            return restVerb switch
            {
                HttpRestVerb.Get => MakeGetRequestAsync(httpClient, uri, cancellationToken),
                HttpRestVerb.Post => MakePostRequestAsync(httpClient, uri, data, cancellationToken),
                HttpRestVerb.Put => MakePutRequestAsync(httpClient, uri, data, cancellationToken),
                HttpRestVerb.Patch => MakePatchRequestAsync(httpClient, uri, data, cancellationToken),
                HttpRestVerb.Delete => MakeDeleteRequestAsync(httpClient, uri, cancellationToken),
                _ => throw new ArgumentOutOfRangeException(nameof(restVerb), $"Unknown {nameof(HttpRestVerb)} type")
            };
        }

        private Task<T> MakeRestRequestAsync<T>(string request, HttpRestVerb restVerb, string httpClientName, object data, string apiVersion, string token, CancellationToken cancellationToken)
        {
            var httpClient = ResolveHttpClient(httpClientName);
            AddApiVersion(httpClient, apiVersion);
            AddBearerToken(httpClient, token);

            var uri = GetUri(request);

            return restVerb switch
            {
                HttpRestVerb.Get => MakeGetRequestAsync<T>(httpClient, uri, cancellationToken),
                HttpRestVerb.Post => MakePostRequestAsync<T>(httpClient, uri, data, cancellationToken),
                HttpRestVerb.Put => MakePutRequestAsync<T>(httpClient, uri, data, cancellationToken),
                HttpRestVerb.Patch => MakePatchRequestAsync<T>(httpClient, uri, data, cancellationToken),
                HttpRestVerb.Delete => MakeDeleteRequestAsync<T>(httpClient, uri, cancellationToken),
                _ => throw new ArgumentOutOfRangeException(nameof(restVerb), $"Unknown {nameof(HttpRestVerb)} type")
            };
        }

        //TODO: add logger
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable 168

        private async Task MakeGetRequestAsync(HttpClient httpClient, Uri uri, CancellationToken cancellationToken)
        {
            try
            {
                var response = await httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                // TODO: log
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
                // TODO: log
                throw;
            }
        }

        private async Task MakePostRequestAsync(HttpClient httpClient, Uri uri, object data, CancellationToken cancellationToken)
        {
            try
            {
                var serializedData = JsonSerializer.Serialize(data);
                using var httpContent = new StringContent(serializedData, Encoding.UTF8, JsonMediaType);
                using var response = await httpClient.PostAsync(uri, httpContent, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                // TODO: log
                throw;
            }
        }

        private async Task<T> MakePostRequestAsync<T>(HttpClient httpClient, Uri uri, object data, CancellationToken cancellationToken)
        {
            try
            {
                var serializedData = JsonSerializer.Serialize(data);
                using var httpContent = new StringContent(serializedData, Encoding.UTF8, JsonMediaType);
                using var response = await httpClient.PostAsync(uri, httpContent, cancellationToken).ConfigureAwait(false);
                return await ProcessHttpRequest<T>(response, cancellationToken).ConfigureAwait(false);
            }

            catch (Exception ex)
            {
                // TODO: log
                throw;
            }
        }

        private async Task MakePutRequestAsync(HttpClient httpClient, Uri uri, object data, CancellationToken cancellationToken)
        {
            try
            {
                var serializedData = JsonSerializer.Serialize(data);
                using var httpContent = new StringContent(serializedData, Encoding.UTF8, JsonMediaType);
                using var response = await httpClient.PutAsync(uri, httpContent, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                // TODO: log
                throw;
            }
        }

        private async Task<T> MakePutRequestAsync<T>(HttpClient httpClient, Uri uri, object data, CancellationToken cancellationToken)
        {
            try
            {
                var serializedData = JsonSerializer.Serialize(data);
                using var httpContent = new StringContent(serializedData, Encoding.UTF8, JsonMediaType);
                using var response = await httpClient.PutAsync(uri, httpContent, cancellationToken).ConfigureAwait(false);
                return await ProcessHttpRequest<T>(response, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // TODO: log
                throw;
            }
        }

        private async Task MakePatchRequestAsync(HttpClient httpClient, Uri uri, object data, CancellationToken cancellationToken)
        {
            try
            {
                var serializedData = JsonSerializer.Serialize(data);
                using var httpContent = new StringContent(serializedData, Encoding.UTF8, JsonMediaType);
                using var response = await httpClient.PatchAsync(uri, httpContent, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                // TODO: log
                throw;
            }
        }

        private async Task<T> MakePatchRequestAsync<T>(HttpClient httpClient, Uri uri, object data, CancellationToken cancellationToken)
        {
            try
            {
                var serializedData = JsonSerializer.Serialize(data);
                using var httpContent = new StringContent(serializedData, Encoding.UTF8, JsonMediaType);
                using var response = await httpClient.PatchAsync(uri, httpContent, cancellationToken).ConfigureAwait(false);
                return await ProcessHttpRequest<T>(response, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // TODO: log
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
                // TODO: log
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
            catch (Exception)
            {
                // TODO: log
                throw;
            }
        }

#pragma warning restore CA1822 // Mark members as static
#pragma warning restore 168

        private HttpClient ResolveHttpClient(string httpClientName) => _httpClientFactory.CreateClient(httpClientName);

        private static void AddApiVersion(HttpClient httpClient, string version)
        {
            if (string.IsNullOrWhiteSpace(version)) return;

            httpClient.DefaultRequestHeaders.Remove(EndpointConstants.ApiVersionHeader);
            httpClient.DefaultRequestHeaders.Add(EndpointConstants.ApiVersionHeader, version);
        }

        private static void AddBearerToken(HttpClient httpClient, string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return;

            httpClient.DefaultRequestHeaders.Remove(EndpointConstants.AuthorizationHeader);
            httpClient.DefaultRequestHeaders.Add(EndpointConstants.AuthorizationHeader, $"{EndpointConstants.BearerHeader}{token}");
        }

        private static Uri GetUri(string request) => new Uri(request, UriKind.Relative);

        private static async Task<T> ProcessHttpRequest<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            // TODO: Switch back to ReadFromJsonAsync
            response.EnsureSuccessStatusCode();
            var streamContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<T>(streamContent, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}

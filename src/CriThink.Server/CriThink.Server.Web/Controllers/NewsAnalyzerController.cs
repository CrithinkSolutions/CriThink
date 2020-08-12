using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using CriThink.Server.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace CriThink.Server.Web.Controllers
{
    /// <summary>
    /// This controller contains APIs to offer services to analyze news and relative URLs
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
    [ApiValidationFilter]
    [ApiController]
    [Route(EndpointConstants.ApiBase + EndpointConstants.NewsAnalyzerBase)] //api/news-analyzer
    public class NewsAnalyzerController : Controller
    {
        private readonly INewsAnalyzerService _newsAnalyzerService;

        public NewsAnalyzerController(INewsAnalyzerService newsAnalyzerService)
        {
            _newsAnalyzerService = newsAnalyzerService ?? throw new ArgumentNullException(nameof(newsAnalyzerService));
        }

        /// <summary>
        /// Perform all the available analysis available in this controller
        /// </summary>
        /// <param name="request">The URI to analyze</param>
        /// <returns>A response with the result of all the performed analysis</returns>
        [Route(EndpointConstants.CompleteAnalysis)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> CompleteAnalysisAsync([FromBody] NewsAnalyzerRequest request)
        {
            var uri = new Uri(request.Uri);
            var responses = await _newsAnalyzerService.GetCompleteAnalysisAsync(uri).ConfigureAwait(false);
            return Ok(new ApiOkResponse(responses));
        }

        /// <summary>
        /// Analyze the given <see cref="Uri"/> and indicate if it has the HTTPS support
        /// </summary>
        /// <param name="request">The URI to analyze</param>
        /// <returns>Provide the analysis result</returns>
        [Route(EndpointConstants.HttpsSupport)] // api/news-analyzer/https-support
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> HasHttpsSupportAsync([FromBody] NewsAnalyzerRequest request)
        {
            var uri = new Uri(request.Uri);
            var hasHttpsSupport = await _newsAnalyzerService.HasUriHttpsSupportAsync(uri).ConfigureAwait(false);
            return Ok(new ApiOkResponse(hasHttpsSupport));
        }

        /// <summary>
        /// Analyze the domain of the given <see cref="Uri"/>
        /// </summary>
        /// <param name="request">The URI to analyze</param>
        /// <returns>Provide the analysis result</returns>
        [Route(EndpointConstants.DomainLookup)] // api/news-analyzer/domain-lookup
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> DomainLookupAsync([FromBody] NewsAnalyzerRequest request)
        {
            var uri = new Uri(request.Uri);
            var domainInfoResponse = await _newsAnalyzerService.AnalyzeDomainAsync(uri).ConfigureAwait(false);
            return Ok(new ApiOkResponse(domainInfoResponse));
        }

        /// <summary>
        /// Parse the news of the given URL and returns news info
        /// </summary>
        /// <param name="request">The news URL</param>
        /// <returns>News information such as author and body</returns>
        [Route(EndpointConstants.ScrapeNews)] // api/news-analyzer/scrape-news
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> ScrapeNewsAsync([FromQuery] ScrapeNewsRequest request)
        {
            var uri = new Uri(request.Uri);
            var response = await _newsAnalyzerService.NewsCheckSpellingAsync(uri).ConfigureAwait(false);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Analyze the given news content and gives scores
        /// </summary>
        /// <param name="request">News uri</param>
        /// <returns>News sentiment scores</returns>
        [Route(EndpointConstants.TextSentimentAnalysis)] // api/news-analyzer/sentiment
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> AnalyzeNewsSentimentAsync([FromQuery] NewsAnalyzerRequest request)
        {
            var uri = new Uri(request.Uri);
            var response = await _newsAnalyzerService.AnalyzeNewsSentimentAsync(uri).ConfigureAwait(false);
            return Ok(new ApiOkResponse(response));
        }
    }
}

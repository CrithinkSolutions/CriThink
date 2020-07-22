using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using CriThink.Server.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        private readonly IDomainAnalyzerService _domainAnalyzerService;
        private readonly ILogger<NewsAnalyzerController> _logger;

        public NewsAnalyzerController(IDomainAnalyzerService domainAnalyzerService, ILogger<NewsAnalyzerController> logger)
        {
            _domainAnalyzerService = domainAnalyzerService ?? throw new ArgumentNullException(nameof(domainAnalyzerService));
            _logger = logger;
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
            var responses = await _domainAnalyzerService.GetCompleteAnalysisAsync(uri).ConfigureAwait(false);
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
            var hasHttpsSupport = await _domainAnalyzerService.HasUriHttpsSupportAsync(uri).ConfigureAwait(false);
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
            var domainInfoResponse = await _domainAnalyzerService.AnalyzeDomainAsync(uri).ConfigureAwait(false);
            return Ok(new ApiOkResponse(domainInfoResponse));
        }
    }
}

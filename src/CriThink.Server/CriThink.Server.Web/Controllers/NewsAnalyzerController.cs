using System;
using CriThink.Common.Endpoints;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using CriThink.Server.Web.Models.DTOs.NewsAnalyzer;
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
        private readonly IDomainAnalyzer _domainAnalyzer;
        private readonly ILogger<NewsAnalyzerController> _logger;

        public NewsAnalyzerController(IDomainAnalyzer domainAnalyzer, ILogger<NewsAnalyzerController> logger)
        {
            _domainAnalyzer = domainAnalyzer ?? throw new ArgumentNullException(nameof(domainAnalyzer));
            _logger = logger;
        }

        [Route(EndpointConstants.HttpsSupport)] // api/news-analyzer/https-support
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [HttpPost]
        public IActionResult HasHttpsSupport([FromBody] HttpsSupportRequest request)
        {
            var uri = new Uri(request.Uri);

            var hasHttpsSupport = _domainAnalyzer.HasUriHttpsSupport(uri);
            return Ok(new ApiOkResponse(hasHttpsSupport));
        }
    }
}

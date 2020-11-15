using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Controllers
{
    /// <summary>
    /// This controller contains demo related APIs
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
    [ApiValidationFilter]
    [ApiController]
    [Route(EndpointConstants.ApiBase + EndpointConstants.DemoBase)] //api/demo
    public class DemoController : Controller
    {
        private readonly INewsAnalyzerService _newsAnalyzerService;

        public DemoController(INewsAnalyzerService newsAnalyzerService)
        {
            _newsAnalyzerService = newsAnalyzerService ?? throw new ArgumentNullException(nameof(newsAnalyzerService));
        }

        /// <summary>
        /// Returns a predefined list of news ready to be analyzed
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route(EndpointConstants.DemoNewsGetAll)] // api/news-analyzer/demo-news
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetNewsListAsync()
        {
            var newsList = await _newsAnalyzerService.GetDemoNewsListAsync().ConfigureAwait(false);
            return Ok(new ApiOkResponse(newsList));
        }

        /// <summary>
        /// Add a news to the predefined list
        /// </summary>
        /// <param name="request">News to add</param>
        /// <returns></returns>
        [Authorize]
        [Route(EndpointConstants.DemoNewsAdd)] // api/news-analyzer/demo-news
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> AddDemoNewsAsync([FromBody] DemoNewsAddRequest request)
        {
            await _newsAnalyzerService.AddDemoNewsAsync(request).ConfigureAwait(false);
            return NoContent();
        }
    }
}

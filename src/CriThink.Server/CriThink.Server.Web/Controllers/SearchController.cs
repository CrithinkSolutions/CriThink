using System;
using System.Net.Mime;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Search;
using CriThink.Server.Application.Queries;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Controllers
{
    [ApiVersion(EndpointConstants.VersionOne)]
    [ApiValidationFilter]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(EndpointConstants.ApiBase + EndpointConstants.SearchBase)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class SearchController : ControllerBase
    {
        private readonly IDebunkingNewsQueries _debunkingNewsQueries;
        private readonly INewsSourceQueries _newsSourceQueries;

        public SearchController(
            IDebunkingNewsQueries debunkingNewsQueries,
            INewsSourceQueries newsSourceQueries)
        {
            _debunkingNewsQueries = debunkingNewsQueries ??
                throw new ArgumentNullException(nameof(debunkingNewsQueries));

            _newsSourceQueries = newsSourceQueries ??
                throw new ArgumentNullException(nameof(newsSourceQueries));
        }

        /// <summary>
        /// Search debunking news and users searches by the given text
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/search?q={text}
        /// 
        /// </remarks>
        /// <response code="200">Returns the search results</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [ProducesResponseType(typeof(SearchByTextResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [HttpGet]
        public async Task<IActionResult> SearchByTextAsync([FromQuery] string query)
        {
            var debunkingNewsResults = await _debunkingNewsQueries.SearchByTextAsync(query);
            var newsSourcesResults = await _newsSourceQueries.SearchInUserSearchesByTextAsync(query);

            var response = new SearchByTextResponse(debunkingNewsResults, newsSourcesResults);
            return Ok(response);
        }
    }
}

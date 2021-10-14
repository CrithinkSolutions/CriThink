using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.DebunkingNews;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Exceptions;
using CriThink.Server.Application.Queries;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CriThink.Server.Web.Controllers
{
    /// <summary>
    /// This controller contains APIs to deal with debunking news
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
    [ApiValidationFilter]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(EndpointConstants.ApiBase + EndpointConstants.DebunkNewsBase)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class DebunkingNewsController : ControllerBase
    {
        private readonly IDebunkingNewsQueries _debunkingNewsQueries;
        private readonly RequestLocalizationOptions _localizationOptions;
        private readonly IMediator _mediator;

        public DebunkingNewsController(
            IDebunkingNewsQueries debunkingNewsQueries,
            IOptions<RequestLocalizationOptions> localizationOptions,
            IMediator mediator)
        {
            _debunkingNewsQueries = debunkingNewsQueries ??
                throw new ArgumentNullException(nameof(debunkingNewsQueries));

            _localizationOptions = localizationOptions.Value;

            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Trigger the update of internal debunking news repository
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST: /api/debunking-news/trigger-update
        /// 
        /// </remarks>
        /// <response code="204">Returns when operation succeeds</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [AllowAnonymous]
        [ServiceFilter(typeof(DebunkingNewsTriggerAuthenticationFilter), Order = int.MinValue)]
        [Route(EndpointConstants.DebunkNewsTriggerUpdate)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status206PartialContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [HttpPost]
        public async Task<IActionResult> TriggerRepositoryUpdateAsync()
        {
            var command = new UpdateDebunkingNewsRepositoryCommand();

            try
            {
                await _mediator.Send(command);

                return NoContent();
            }
            catch (DebunkingNewsFetcherPartialFailureException)
            {
                return StatusCode(StatusCodes.Status206PartialContent);
            }
        }

        /// <summary>
        /// Get all the debunking news
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET: /api/debunking-news/all?{pageSize}{pageIndex}
        /// 
        /// </remarks>
        /// <param name="request">Page index and number of debunking news per page</param>
        /// <response code="200">Returns the list of debunking news</response>
        /// <response code="400">If the request query string is invalid</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [Route(EndpointConstants.DebunkingNewsGetAll)]
        [ProducesResponseType(typeof(DebunkingNewsGetAllResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [HttpGet]
        public async Task<IActionResult> GetAllDebunkingNewsAsync([FromQuery] DebunkingNewsGetAllRequest request)
        {
            string languageFilter = null;

            var hasLanguage = Request.Headers.TryGetValue("Accept-Language", out var languageValues);
            if (hasLanguage && languageValues.Any())
            {
                var commonLanguages = _localizationOptions.SupportedCultures
                    .Select(sc => sc.TwoLetterISOLanguageName.ToLowerInvariant())
                    .Intersect(languageValues)
                    .ToList();

                if (commonLanguages.Any())
                {
                    languageFilter = commonLanguages
                            .Aggregate((i, j) => $"{i},{j}");
                }
            }

            var allDebunkingNews = await _debunkingNewsQueries.GetAllDebunkingNewsAsync(
                request.PageSize,
                request.PageIndex,
                languageFilter);

            return Ok(new ApiOkResponse(allDebunkingNews));
        }

        /// <summary>
        /// Get the specified debunking news
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: /api/debunking-news?{id}
        /// 
        /// </remarks>
        /// <param name="request">Debunking news id</param>
        /// <response code="200">Returns the requested debunking news</response>
        /// <response code="400">If the request query string is invalid</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="404">If the given id is not found</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [ProducesResponseType(typeof(DebunkingNewsGetDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [HttpGet]
        public async Task<IActionResult> GetDebunkingNewsAsync([FromQuery] DebunkingNewsGetRequest request)
        {
            var debunkingNews = await _debunkingNewsQueries.GetDebunkingNewsByIdAsync(request.Id);
            return Ok(new ApiOkResponse(debunkingNews));
        }
    }
}

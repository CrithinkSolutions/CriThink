using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Queries;
using CriThink.Server.Core.Commands;
using CriThink.Server.Infrastructure.ExtensionMethods;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Controllers
{
    /// <summary>
    /// This controller contains API to manage black and whitelist
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
    [ApiValidationFilter]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(EndpointConstants.ApiBase + EndpointConstants.NewsSourceBase)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class NewsSourceController : ControllerBase
    {
        private readonly INewsSourceQueries _newsSourceQueries;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public NewsSourceController(
            INewsSourceQueries newsSourceQueries,
            IMapper mapper,
            IMediator mediator)
        {
            _newsSourceQueries = newsSourceQueries ??
                throw new ArgumentNullException(nameof(newsSourceQueries));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Gets list of questions for a news source
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/news-source/question/all
        ///     
        /// </remarks>
        /// <response code="200">Returns the list of questions</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="404">If the given domain is not found</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [ProducesResponseType(typeof(NewsSourceGetAllQuestionsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Route(EndpointConstants.NewsSourceAllQuestions)]
        [HttpGet]
        public async Task<IActionResult> GetNewsSourceQuestionsAsync()
        {
            var questions = await _newsSourceQueries.GetGeneralQuestionsAsync();
            return Ok(new ApiOkResponse(questions));
        }

        /// <summary>
        /// Gives answers to questions for the specified news source. Returns
        /// your rate, the community rate, the news source category with
        /// the description, the related debunking news (if any)
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST: /api/news-source/question
        ///     {
        ///         "newsLink": "link",
        ///         "questions": [
        ///             {
        ///                 "questionId": "id",
        ///                 "rate": rate
        ///             }
        ///         ]
        ///     }
        ///     
        /// </remarks>
        /// <param name="request">The answers to article questions</param>
        /// <response code="200">Returns the user calculated rate, the
        /// community rate, the news source category and description,
        /// the related debunking news (if any)</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="404">If the given domain is not found</response>
        /// <response code="409">If the user has already gave a rate for this news</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [ProducesResponseType(typeof(NewsSourcePostAnswersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Route(EndpointConstants.NewsSourcePostAnswerToQuestions)]
        [HttpPost]
        public async Task<IActionResult> PostAnswersToArticleQuestionsAsync([FromBody] NewsSourcePostAllAnswersRequest request)
        {
            try
            {
                var userId = User.GetId();
                var userEmail = User.GetEmail();

                var command = new CreateAnswersToNewsSourceQuestionsCommand(
                    userId,
                    userEmail,
                    request.NewsLink,
                    request.Questions);

                var response = await _mediator.Send(command);

                return Ok(new ApiOkResponse(response));
            }
            catch (InvalidOperationException)
            {
                return Conflict();
            }
        }

        /// <summary>
        /// Adds many news sources in a single call. If a resource already exists, its value
        /// is updated or unchanged following the business rules. If the request contains a
        /// duplicated key, only the first one is considered
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT: /api/news-source/add/batch
        ///     {
        ///         "newsSources": {
        ///             "key1": "classification1",
        ///             "key2": "classification2"
        ///         }
        ///     }
        ///     
        /// </remarks>
        /// <param name="request">A dictionary representing the news link and the
        /// classification</param>
        /// <response code="204">Returns when operation succeeds</response>
        /// <response code="206">Returns when the operation partially succeeeds</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [AllowAnonymous]
        [ServiceFilter(typeof(ScraperAuthenticationFilter), Order = int.MinValue)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status206PartialContent)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Route(EndpointConstants.NewsSourceAddBatch)]
        [HttpPut]
        public async Task<IActionResult> AddBatchNewsSourceAsync([FromBody] NewsSourceAddBatchRequest request)
        {
            NewsSourceAddBatchResponse failedSources = null;
            var anyValidSource = false;

            foreach (var kvp in request.NewsSources)
            {
                try
                {
                    var command = new CreateNewsSourceCommand(
                        kvp.Key, _mapper.Map<NewsSourceClassification, NewsSourceAuthenticity>(kvp.Value));

                    await _mediator.Send(command);

                    anyValidSource = true;
                }
                catch (Exception ex)
                {
                    failedSources ??= new NewsSourceAddBatchResponse();
                    failedSources.Errors.Add(kvp.Key, ex.Message);
                }
            }

            if (failedSources?.Errors?.Any() == true)
            {
                return anyValidSource ?
                    StatusCode(StatusCodes.Status206PartialContent, failedSources) :
                    BadRequest(new { message = "The request is invalid", failedSources.Errors });
            }

            return NoContent();
        }
    }
}

using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Article;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Models.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Controllers
{
    /// <summary>
    /// This controller contains API to interact with a specific article
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
    [ApiValidationFilter]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(EndpointConstants.ApiBase + EndpointConstants.ArticleBase)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
        }

        /// <summary>
        /// Gets list of questions for an article
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/article/question/all
        ///     
        /// </remarks>
        /// <response code="200">Returns the list of questions</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="404">If the given domain is not found</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [ProducesResponseType(typeof(ArticleGetAllQuestionsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Route(EndpointConstants.ArticleAllQuestions)]
        [HttpGet]
        public async Task<IActionResult> GetArticleQuestionsAsync()
        {
            var questions = await _articleService.GetQuestionsAsync();
            return Ok(new ApiOkResponse(questions));
        }

        /// <summary>
        /// Gives answers to questions for the specified article. Returns
        /// your rate, the community rate, the news source category with
        /// the description, the related debunking news (if any)
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST: /api/article/question
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
        [ProducesResponseType(typeof(ArticlePostAnswersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Route(EndpointConstants.ArticlePostAnswerToQuestions)]
        [HttpPost]
        public async Task<IActionResult> PostAnswersToArticleQuestionsAsync([FromBody] ArticlePostAllAnswersRequest request)
        {
            try
            {
                var result = await _articleService.PostAnswersToArticleQuestionsAsync(request);
                return Ok(new ApiOkResponse(result));
            }
            catch (InvalidOperationException)
            {
                return Conflict();
            }
        }
    }
}

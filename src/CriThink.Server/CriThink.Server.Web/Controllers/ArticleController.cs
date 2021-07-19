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
        /// <response code="400">If the request body is invalid</response>
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
    }
}

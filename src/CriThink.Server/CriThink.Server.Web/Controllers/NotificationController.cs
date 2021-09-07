using System;
using System.Net.Mime;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Notification;
using CriThink.Server.Application.Commands;
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
    /// This controller contains API to manage user notifications
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
    [ApiValidationFilter]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(EndpointConstants.ApiBase + EndpointConstants.NotificationBase)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(
            IMediator mediator)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Register the user for being notified when a news source is analyzed
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: /api/notification/unknown-source
        ///     {
        ///         "uri": "uri",
        ///     }
        ///         
        /// </remarks>
        /// <param name="request">The unknown uri</param>
        /// <response code="204">Returns when operation succeeds</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="404">If the given domain is not found</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Route(EndpointConstants.NotificationUnknownNewsSource)]
        [HttpPost]
        public async Task<IActionResult> RequestNotificationForUnknownSourceAsync([FromBody] NewsSourceNotificationForUnknownDomainRequest request)
        {
            var userEmail = User.GetEmail();

            var command = new CreateNotificationForUnknownSourceCommand(
                request.NewsSource,
                userEmail);

            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Cancel the user registration to the notification sent when a news source
        /// is analyzed
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PATCH: /api/notification/unknown-source
        ///     {
        ///         "uri": "uri"
        ///     }
        /// 
        /// </remarks>
        /// <param name="request">The unknown uri</param>
        /// <response code="204">Returns when operation succeeds</response>
        /// <response code="400">If the request body is invalid</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="404">If the given domain is not found</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status503ServiceUnavailable)]
        [Route(EndpointConstants.NotificationUnknownNewsSource)]
        [HttpPatch]
        public async Task<IActionResult> CancelNotificationForUnknownSourceAsync([FromBody] NewsSourceCancelNotificationForUnknownDomainRequest request)
        {
            var userEmail = User.GetEmail();

            var command = new DeleteNotificationForUnknownSourceCommand(
                request.NewsSource,
                userEmail);

            await _mediator.Send(command);

            return NoContent();
        }
    }
}

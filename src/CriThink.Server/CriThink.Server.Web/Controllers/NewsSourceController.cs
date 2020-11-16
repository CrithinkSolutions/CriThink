﻿using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Common;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.NewsSource.Requests;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Web.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace CriThink.Server.Web.Controllers
{
    /// <summary>
    /// This controller contains API to manage black and whitelist
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
    [ApiValidationFilter]
    [ApiController]
    [Route(EndpointConstants.ApiBase + EndpointConstants.NewsSourceBase)] //api/news-source
    public class NewsSourceController : Controller
    {
        private readonly INewsSourceService _newsSourceService;

        public NewsSourceController(INewsSourceService newsSourceService)
        {
            _newsSourceService = newsSourceService ?? throw new ArgumentNullException(nameof(newsSourceService));
        }

        /// <summary>
        /// Add the given source to white or black list
        /// </summary>
        /// <param name="request">Source to add</param>
        /// <returns>Returns the operation result</returns>
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost] // api/news-source/
        public async Task<IActionResult> AddSourceAsync([FromBody] NewsSourceAddRequest request)
        {
            await _newsSourceService.AddSourceAsync(request).ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Remove the given source from the whitelist
        /// </summary>
        /// <param name="request">Source to remove</param>
        /// <returns>Returns the operation result</returns>
        [Authorize]
        [Route(EndpointConstants.NewsSourceRemoveWhiteNewsSource)] // api/news-source/whitelist
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<IActionResult> RemoveGoodNewsSourceAsync([FromBody] SimpleUriRequest request)
        {
            var uri = new Uri(request.Uri);
            await _newsSourceService.RemoveGoodNewsSourceAsync(uri).ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Remove the given source from the blacklist
        /// </summary>
        /// <param name="request">Source to remove</param>
        /// <returns>Returns the operation result</returns>
        [Authorize]
        [Route(EndpointConstants.NewsSourceRemoveBlackNewsSource)] // api/news-source/blacklist
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<IActionResult> RemoveBadNewsSourceAsync([FromBody] SimpleUriRequest request)
        {
            var uri = new Uri(request.Uri);
            await _newsSourceService.RemoveBadSourceAsync(uri).ConfigureAwait(false);
            return NoContent();
        }

        /// <summary>
        /// Search the given source and returns the list that contains it
        /// </summary>
        /// <param name="request">Source to search</param>
        /// <returns>Returns the list where the source is contained</returns>
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [HttpGet] // api/news-source/
        public async Task<IActionResult> SearchNewsSourceAsync([FromQuery] SimpleUriRequest request)
        {
            var uri = new Uri(request.Uri);
            var searchResponse = await _newsSourceService.SearchNewsSourceAsync(uri).ConfigureAwait(false);
            return Ok(searchResponse);
        }

        /// <summary>
        /// Get all the news sources stored. Results can be filtered
        /// </summary>
        /// <param name="request">Optional filter</param>
        /// <returns>All the news sources</returns>
        [AllowAnonymous]
        [Route(EndpointConstants.NewsSourceNewsSourceGetAll)] // api/news-source/all
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetAllNewsSourcesAsync([FromQuery] NewsSourceGetAllFilterRequest request = NewsSourceGetAllFilterRequest.None)
        {
            var results = await _newsSourceService.GetAllNewsSourcesAsync(request).ConfigureAwait(false);
            return Ok(results);
        }
    }
}

using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.NewsSource.Requests;
using CriThink.Server.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Controllers
{
    /// <summary>
    /// This controller contains API to manage black and whitelist
    /// </summary>
    [ApiVersion(EndpointConstants.VersionOne)]
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
        /// Add the given source to white or blacklist
        /// </summary>
        /// <param name="request">Source to add</param>
        /// <returns>Returns the operation result</returns>
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(int), 500)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> AddSourceAsync([FromBody] NewsSourceAddRequest request)
        {
            await _newsSourceService.AddSourceAsync(request).ConfigureAwait(false);
            return Ok();
        }

        /// <summary>
        /// Remove the given source from the whitelist
        /// </summary>
        /// <param name="request">Source to remove</param>
        /// <returns>Returns the operation result</returns>
        [Route(EndpointConstants.RemoveGoodNewsSource)] // api/news-source/good
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(int), 404)]
        [ProducesResponseType(typeof(int), 500)]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<IActionResult> RemoveGoodNewsSourceAsync([FromBody] NewsSourceRemoveRequest request)
        {
            await _newsSourceService.RemoveGoodNewsSourceAsync(request).ConfigureAwait(false);
            return Ok();
        }

        /// <summary>
        /// Remove the given source from the blacklist
        /// </summary>
        /// <param name="request">Source to remove</param>
        /// <returns>Returns the operation result</returns>
        [Route(EndpointConstants.RemoveBadNewsSource)] // api/news-source/bad
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(int), 404)]
        [ProducesResponseType(typeof(int), 500)]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<IActionResult> RemoveBadNewsSourceAsync([FromBody] NewsSourceRemoveRequest request)
        {
            await _newsSourceService.RemoveBadSourceAsync(request).ConfigureAwait(false);
            return Ok();
        }

        /// <summary>
        /// Search the given source and returns the list that contains it
        /// </summary>
        /// <param name="request">Source to search</param>
        /// <returns>Returns the list where the source is contained</returns>
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(int), 404)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> SearchNewsSourceAsync([FromQuery] NewsSourceSearchRequest request)
        {
            var searchResponse = await _newsSourceService.SearchNewsSourceAsync(request).ConfigureAwait(false);
            return Ok(searchResponse);
        }

        /// <summary>
        /// Get all the news sources stored. Result can be filtered
        /// </summary>
        /// <param name="request">Optional filter</param>
        /// <returns>All the news sources</returns>
        [Route(EndpointConstants.NewsSourceGetAll)]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(int), 500)]
        [HttpGet]
        public async Task<IActionResult> GetAllNewsSourcesAsync([FromQuery] NewsSourceGetAllFilterRequest request = NewsSourceGetAllFilterRequest.None)
        {
            var results = await _newsSourceService.GetAllNewsSourcesAsync(request).ConfigureAwait(false);
            return Ok(results);
        }
    }
}

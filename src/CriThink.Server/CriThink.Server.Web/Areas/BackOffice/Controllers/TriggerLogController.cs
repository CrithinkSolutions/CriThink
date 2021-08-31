using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Application.Queries;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Areas.BackOffice.Controllers
{
    /// <summary>
    /// Controller to handle the backoffice trigger log operations
    /// </summary>
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
    [Area("BackOffice")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route(EndpointConstants.TriggerLogBase)]
    public class TriggerLogController : Controller
    {
        private readonly IDebunkingNewsTriggerLogQueries _debunkingNewsTriggerLogQueries;

        public TriggerLogController(
            IDebunkingNewsTriggerLogQueries debunkingNewsTriggerLogQueries)
        {
            _debunkingNewsTriggerLogQueries = debunkingNewsTriggerLogQueries
                ?? throw new ArgumentNullException(nameof(debunkingNewsTriggerLogQueries));
        }

        /// <summary>
        /// Returns the trigger log section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(SimplePaginationViewModel viewModel)
        {
            var logs = await _debunkingNewsTriggerLogQueries.GetAllTriggerLogsAsync(
                viewModel.PageIndex,
                viewModel.PageSize);

            return View(logs);
        }
    }
}
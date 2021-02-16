using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using CriThink.Server.Web.Facades;
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
    public class TriggerLogController : Controller
    {
        private readonly ITriggerLogServiceFacade _triggerLogServiceFacade;

        public TriggerLogController(ITriggerLogServiceFacade triggerLogServiceFacade)
        {
            _triggerLogServiceFacade = triggerLogServiceFacade ?? throw new ArgumentNullException(nameof(triggerLogServiceFacade));
        }

        /// <summary>
        /// Returns the trigger log section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(EndpointConstants.TriggerLogBase)]
        public async Task<IActionResult> Index(SimplePaginationViewModel viewModel)
        {
            var log = await _triggerLogServiceFacade.GetAllTriggerLogAsync(viewModel).ConfigureAwait(false);
            return View(log);
        }
    }
}
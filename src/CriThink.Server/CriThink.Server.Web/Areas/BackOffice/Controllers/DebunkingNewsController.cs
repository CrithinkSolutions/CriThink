using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Queries;
using CriThink.Server.Domain.Exceptions;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.DebunkingNews;
using CriThink.Server.Web.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CriThink.Server.Web.Areas.BackOffice.Controllers
{
    /// <summary>
    /// Controller to handle the backoffice debunking news operations
    /// </summary>
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RoleNames.Admin)]
    [Area("BackOffice")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route(EndpointConstants.DebunkNewsBase)]
    public class DebunkingNewsController : Controller
    {
        private readonly IDebunkingNewsQueries _debunkingNewsQueries;
        private readonly IDebunkingNewsPublisherQueries _debunkingNewsPublisherQueries;
        private readonly IMediator _mediator;

        public DebunkingNewsController(
            IDebunkingNewsQueries debunkingNewsQueries,
            IDebunkingNewsPublisherQueries debunkingNewsPublisherQueries,
            IMediator mediator)
        {
            _debunkingNewsQueries = debunkingNewsQueries ??
                throw new ArgumentNullException(nameof(debunkingNewsQueries));

            _debunkingNewsPublisherQueries = debunkingNewsPublisherQueries ??
                throw new ArgumentNullException(nameof(debunkingNewsPublisherQueries));

            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Returns the debunking news section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(SimplePaginationViewModel viewModel)
        {
            var news = await _debunkingNewsQueries.GetAllDebunkingNewsAsync(
                viewModel.PageSize,
                viewModel.PageIndex);

            return View(news);
        }

        /// <summary>
        /// Returns the add debunking news page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(EndpointConstants.DebunkingNewsAddNews)]
        public async Task<ActionResult> AddNewsView()
        {
            var publishers = await _debunkingNewsPublisherQueries.GetAllDebunkingNewsPublishersAsync();

            IEnumerable<SelectListItem> categories = publishers
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name,
                    Selected = false
                }).ToList();

            var viewModel = new AddNewsViewModel
            {
                Publishers = categories
            };

            return View("AddNewsView", viewModel);
        }

        /// <summary>
        /// Add debunking news
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.DebunkingNewsAddNews)]
        public async Task<IActionResult> AddNewsAsync(AddNewsViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (!ModelState.IsValid)
            {
                return View("AddNewsView", viewModel);
            }

            try
            {
                var sel = viewModel.Publishers.Single(x => x.Selected);
                var publisherId = Guid.Parse(sel.Value);

                var command = new CreateDebunkingNewsCommand(
                    viewModel.Caption,
                    viewModel.Link,
                    viewModel.Keywords,
                    viewModel.Title,
                    viewModel.ImageLink,
                    publisherId);

                await _mediator.Send(command);

                viewModel.Message = "News Added!";
                return View("AddNewsView", viewModel);
            }
            catch (CriThinkNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Remove debunking news
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.DebunkingNewsRemoveNews)]
        public async Task<IActionResult> RemoveNewsAsync(SimpleDebunkingNewsViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            try
            {
                if (ModelState.IsValid)
                {
                    var command = new DeleteDebunkingNewsCommand(viewModel.Id);
                    await _mediator.Send(command);
                }

                return RedirectToAction("Index");
            }
            catch (CriThinkNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Info debunking news by id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.DebunkingNewsInfoNews)]
        public async Task<IActionResult> GetDebunkingNewsAsync(SimpleDebunkingNewsViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            try
            {
                var debunkingNews = await _debunkingNewsQueries.GetDebunkingNewsByIdAsync(
                    viewModel.Id);

                return Ok(new ApiOkResponse(debunkingNews));
            }
            catch (CriThinkNotFoundException)
            {
                return NotFound();
            }
        }

        [Route(EndpointConstants.MvcEdit)]
        [HttpGet]
        public async Task<IActionResult> UpdateDebunkingNewsAsync(Guid id)
        {
            var debunkingNews = await _debunkingNewsQueries.GetDebunkingNewsByIdAsync(id);
            if (debunkingNews is not null)
            {
                var viewModel = new UpdateDebunkingNewsViewModel
                {
                    Title = debunkingNews.Title,
                    Caption = debunkingNews.Caption,
                    Link = debunkingNews.Link,
                    ImageLink = debunkingNews.ImageLink,
                    Keywords = string.Join(", ", debunkingNews.Keywords),
                };

                return View(viewModel);
            }

            else
                return NotFound();
        }

        [Route(EndpointConstants.MvcEdit)]
        [HttpPost]
        public async Task<IActionResult> PostUpdateDebunkingNewsAsync(UpdateDebunkingNewsViewModel viewModel)
        {
            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            if (!ModelState.IsValid)
                return View(viewModel);

            var command = new UpdateDebunkingNewsCommand(
                viewModel.Id,
                viewModel.Title,
                viewModel.Caption,
                viewModel.Link,
                viewModel.ImageLink);

            await _mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }
    }
}

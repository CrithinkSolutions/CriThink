using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Queries;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.QueryResults;
using CriThink.Server.Infrastructure.Constants;
using CriThink.Server.Web.Areas.BackOffice.ViewModels;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CriThink.Server.Web.Areas.BackOffice.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RoleConstants.AdminRole)]
    [Route(EndpointConstants.NewsSourceBase)]
    [Area("BackOffice")]
    public class NewsSourceController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly INotificationQueries _notificationQueries;
        private readonly INewsSourceQueries _newsSourceQueries;

        public NewsSourceController(
            IMapper mapper,
            IMediator mediator,
            INotificationQueries notificationQueries,
            INewsSourceQueries newsSourceQueries)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));

            _notificationQueries = notificationQueries ??
                throw new ArgumentNullException(nameof(notificationQueries));

            _newsSourceQueries = newsSourceQueries ??
                throw new ArgumentNullException(nameof(newsSourceQueries));
        }

        [HttpGet]
        public IActionResult Index(SimplePaginationViewModel viewModel)
        {
            var queryResult = _newsSourceQueries.GetAllNewsSources(
                viewModel.PageSize,
                viewModel.PageIndex,
                NewsSourceAuthenticityFilter.All);

            var indexViewModel = new IndexViewModel(
                _mapper.Map<IEnumerable<GetAllNewsSourceQueryResult>, ICollection<NewsSourceViewModel>>(
                        queryResult.Take(viewModel.PageSize)),
                queryResult.Count > viewModel.PageSize);

            return View(indexViewModel);
        }

        [Route(EndpointConstants.MvcAdd)]  // news-source/add
        [HttpGet]
        public IActionResult AddSource()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.MvcAdd)]  // news-source/add
        [HttpPost]
        public async Task<IActionResult> AddSource(AddNewsSourceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var command = new CreateNewsSourceCommand(
                viewModel.Uri,
                _mapper.Map<Classification, NewsSourceAuthenticity>(viewModel.Classification));

            try
            {
                await _mediator.Send(command);
                viewModel.Message = $"Source '{viewModel.Uri}' successfully added";
                return View(viewModel);
            }
            catch (Exception)
            {
                viewModel.Message = $"Something went wrong adding '{viewModel.Uri}'";
                return View(viewModel);
            }
        }

        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.NewsSourceRemoveNewsSource)] // news-source/blacklist
        [HttpDelete]
        public async Task<IActionResult> RemoveNewsSourceAsync(RemoveBlacklistViewModel viewModel)
        {
            var command = new DeleteNewsSourceCommand(
                viewModel.Uri);

            await _mediator.Send(command);

            return NoContent();
        }

        [Route(EndpointConstants.MvcEdit)] // news-source/edit
        [HttpGet]
        public Task<IActionResult> Edit(string newsSourceLink)
        {
            throw new NotImplementedException();
            //if (string.IsNullOrEmpty(newsSourceLink))
            //    return RedirectToAction(nameof(Index));

            //var searchResult = await _newsSourceQueries.GetNewsSourceByNameAsync(newsSourceLink);
            //if (searchResult is null)
            //    return RedirectToAction(nameof(Index));

            //var viewModel = new EditNewsSourceViewModel
            //{
            //    OldLink = searchResult.,
            //    NewLink = searchResult.Uri,
            //    Classification = searchResult.Classification,
            //};

            //return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.MvcEdit)] // news-source/edit
        [HttpPost]
        public async Task<IActionResult> Edit(EditNewsSourceViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var searchResult = await _newsSourceQueries.GetNewsSourceByNameAsync(viewModel.OldLink);
            if (searchResult is null)
                return RedirectToAction(nameof(Index));

            var deleteCommand = new DeleteNewsSourceCommand(
                viewModel.OldLink);

            await _mediator.Send(deleteCommand);

            var createCommand = new CreateNewsSourceCommand(
                viewModel.NewLink,
                _mapper.Map<Classification, NewsSourceAuthenticity>(viewModel.Classification));

            await _mediator.Send(createCommand);

            return RedirectToAction(nameof(Index));
        }

        [Route(EndpointConstants.NewsSourcesGetAllNotifications)] // news-source/notification-requests
        [HttpGet]
        public async Task<IActionResult> GetNotificationRequestsAsync(SimplePaginationViewModel viewModel)
        {
            var response = await _notificationQueries.GetAllNotificationsAsync(
                viewModel.PageSize,
                viewModel.PageIndex);

            return View("NotificationRequests", response);
        }

        [Route(EndpointConstants.NewsSourcesGetAllUnknownNewsSources)] // news-source/get-all-unknown
        [HttpGet]
        public async Task<IActionResult> GetUnknownNewsSourcesAsync(SimplePaginationViewModel viewModel)
        {
            var response = await _newsSourceQueries.GetAllUnknownNewsSourcesAsync(
                viewModel.PageSize,
                viewModel.PageIndex);

            return View("Unknown", response);
        }

        [Route(EndpointConstants.NewsSourceTriggerIdentifiedSource)] // news-source/identify
        [HttpGet]
        public async Task<IActionResult> Identify(Guid id)
        {
            var result = await _newsSourceQueries.GetUnknownNewsSourceByIdAsync(id);

            var viewModel = new UnknownNewsSourceViewModel
            {
                Id = result.Id,
                Classification = _mapper.Map<NewsSourceClassification, Classification>(result.Classification),
                Source = result.Uri,
            };

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Route(EndpointConstants.NewsSourceTriggerIdentifiedSource)] // news-source/identify
        [HttpPost]
        public async Task<IActionResult> Identify(UnknownNewsSourceViewModel viewModel)
        {
            if (viewModel.Classification == Classification.Unknown)
                ModelState.AddModelError(nameof(UnknownNewsSourceViewModel.Classification), "You must identify the source");

            if (!ModelState.IsValid)
                return View(viewModel);

            var command = new IdentifyUnknownNewsSourceCommand(
                viewModel.Source,
                _mapper.Map<Classification, NewsSourceAuthenticity>(viewModel.Classification));

            await _mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Services;
using CriThink.Server.Infrastructure.ExtensionMethods;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class UpdateUserProfileAvatarCommandHandler : IRequestHandler<UpdateUserProfileAvatarCommand>
    {
        private readonly IUserAvatarService _userAvatarService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<UpdateUserProfileAvatarCommandHandler> _logger;

        public UpdateUserProfileAvatarCommandHandler(
            IHttpContextAccessor httpContext,
            IUserAvatarService userAvatarService,
            ILogger<UpdateUserProfileAvatarCommandHandler> logger)
        {
            _httpContext = httpContext ??
                throw new ArgumentNullException(nameof(httpContext));

            _userAvatarService = userAvatarService ??
                throw new ArgumentNullException(nameof(userAvatarService));

            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserProfileAvatarCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("Updating UserProfile Avatar");

            var userId = _httpContext.HttpContext.User.GetId();

            await _userAvatarService.UpdateUserProfileAvatarAsync(userId, request.FormFile, cancellationToken);

            return Unit.Value;
        }
    }
}

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Core.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContext, ILogger<UserProfileService> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _logger = logger;
        }

        public async Task UpdateUserProfileAsync(UserProfileUpdateRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var userId = _httpContext?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ??
                throw new InvalidOperationException("Token does not contain any valid claim");

            _logger?.LogInformation("Requested user profile update", userId);

            var userProfileUpToDate = _mapper.Map<UserProfileUpdateRequest, UserProfile>(request);

            userProfileUpToDate.UserId = Guid.Parse(userId);

            var command = new UpdateUserProfileCommand(userProfileUpToDate);

            _ = await _mediator.Send(command).ConfigureAwait(false);

            _logger?.LogInformation("User profile updated", userId);
        }
    }
}

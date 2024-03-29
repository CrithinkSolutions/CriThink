﻿using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.Exceptions;
using CriThink.Server.Domain.Interfaces;
using CriThink.Server.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class VerifyAccountEmailCommandHandler : IRequestHandler<VerifyAccountEmailCommand, VerifyUserEmailResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtManager _jwtManager;
        private readonly ILogger<VerifyAccountEmailCommandHandler> _logger;
        private readonly HttpContext _httpContext;

        public VerifyAccountEmailCommandHandler(
            IUserRepository userRepository,
            IJwtManager jwtManager,
            IHttpContextAccessor httpContext,
            ILogger<VerifyAccountEmailCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _jwtManager = jwtManager ??
                throw new ArgumentNullException(nameof(jwtManager));

            _httpContext = httpContext.HttpContext;

            _logger = logger;
        }

        public async Task<VerifyUserEmailResponse> Handle(VerifyAccountEmailCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("VerifyUserEmail");

            var userId = request.UserId;
            var confirmationCode = request.Code;

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user is null)
                throw new CriThinkNotFoundException("The user doesn't exists", $"UserId: '{userId}'");

            if (user.IsDeleted)
                throw new InvalidOperationException("The user is disabled");

            var result = await _userRepository.ConfirmUserEmailAsync(user, confirmationCode);
            if (!result.Succeeded)
            {
                var ex = new CriThinkIdentityOperationException(result);

                _logger?.LogError(
                    ex,
                    "Error verifying user email {0} {1}",
                    user.Email,
                    confirmationCode);

                throw ex;
            }

            var refreshToken = _jwtManager.GenerateRefreshToken();
            var lifetimeFromNow = _jwtManager.GetDefaultRefreshTokenLifetime();
            user.AddRefreshToken(refreshToken, _httpContext?.Connection.RemoteIpAddress?.ToString(), lifetimeFromNow);

            await _userRepository.UpdateUserAsync(user);

            var jwtToken = await _jwtManager.GenerateUserJwtTokenAsync(user);

            _logger?.LogInformation("VerifyUserEmail: done");

            return new VerifyUserEmailResponse
            {
                UserId = userId,
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
                UserEmail = user.Email,
                Username = user.UserName
            };
        }
    }
}

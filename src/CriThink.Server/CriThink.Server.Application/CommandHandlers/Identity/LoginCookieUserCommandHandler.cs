using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.Exceptions;
using CriThink.Server.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class LoginCookieUserCommandHandler : IRequestHandler<LoginCookieUserCommand, ClaimsIdentity>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<LoginCookieUserCommandHandler> _logger;

        public LoginCookieUserCommandHandler(
            IUserRepository userRepository,
            ILogger<LoginCookieUserCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _logger = logger;
        }

        public async Task<ClaimsIdentity> Handle(LoginCookieUserCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("LoginCookieUser");

            var user = await _userRepository.FindUserAsync(request.Email ?? request.Username);
            if (user is null)
                throw new CriThinkNotFoundException("The user doesn't exists");
            if (user.IsDeleted)
                throw new InvalidOperationException("The user is disabled");

            var result = await _userRepository.PasswordSignInAsync(
                user,
                request.Password,
                request.RememberMe,
                false);

            ProcessPasswordVerificationResult(result);

            var userClaims = await _userRepository.GetUserClaimsAsync(user);

            _logger?.LogInformation("LoginCookieUser: done");

            return new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private static void ProcessPasswordVerificationResult(SignInResult signInResult)
        {
            if (!signInResult.Succeeded)
                throw new CriThinkNotFoundException("Password is not correct");
        }
    }
}

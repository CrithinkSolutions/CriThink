using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Application.CommandHandlers
{
    internal abstract class BaseUserCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected BaseUserCommandHandler(
            IUserRepository userRepository,
            IJwtManager jwtManager,
            IHttpContextAccessor httpContext)
        {
            UserRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            JwtManager = jwtManager ??
                throw new ArgumentNullException(nameof(jwtManager));

            HttpContext = httpContext.HttpContext;
        }

        protected IUserRepository UserRepository { get; }

        protected IJwtManager JwtManager { get; }

        protected HttpContext HttpContext { get; }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

        protected async Task AddRefreshTokenToUserAsync(string refreshToken, User user)
        {
            var lifetimeFromNow = JwtManager.GetDefaultRefreshTokenLifetime();
            user.AddRefreshToken(refreshToken, HttpContext?.Connection.RemoteIpAddress?.ToString(), lifetimeFromNow);

            var result = await UserRepository.UpdateUserAsync(user);

            if (!result.Succeeded)
                throw new InvalidOperationException("Error assigning the refresh token to user");
        }
    }
}

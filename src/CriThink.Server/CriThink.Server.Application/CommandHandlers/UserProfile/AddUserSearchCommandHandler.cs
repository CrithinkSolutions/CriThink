using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Exceptions;
using CriThink.Server.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers.UserProfile
{
    internal class AddUserSearchCommandHandler : IRequestHandler<AddUserSearchCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AddUserSearchCommandHandler> _logger;

        public AddUserSearchCommandHandler(
            IUserRepository userRepository,
            ILogger<AddUserSearchCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(AddUserSearchCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;

            var user = await _userRepository.GetUserByIdAsync(userId, true, cancellationToken);
            if (user is null)
                throw new CriThinkNotFoundException("User not found", userId);

            var searchedNews = UserSearch.CreateTextSearch(
                userId: userId,
                searchedText: request.SearchedText);

            user.AddSearch(searchedNews);

            await _userRepository.UpdateUserAsync(user);

            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

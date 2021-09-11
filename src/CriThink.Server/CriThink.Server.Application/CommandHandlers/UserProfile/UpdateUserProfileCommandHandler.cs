using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.Exceptions;
using CriThink.Server.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

        public UpdateUserProfileCommandHandler(
            IUserRepository userRepository,
            ILogger<UpdateUserProfileCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(
            UpdateUserProfileCommand request,
            CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(UpdateUserProfileCommand), request.UserId);

            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user is null)
                throw new CriThinkNotFoundException("User not found", request.UserId);

            user.UpdateFamilyName(request.FamilyName);
            user.UpdateGivenName(request.GivenName);
            user.UpdateDescription(request.Description);
            user.UpdateGender(request.Gender);
            user.UpdateCountry(request.Country);
            user.UpdateTelegram(request.Telegram);
            user.UpdateSkype(request.Skype);
            user.UpdateTwitter(request.Twitter);
            user.UpdateInstagram(request.Instagram);
            user.UpdateFacebook(request.Facebook);
            user.UpdateSnapchat(request.Snapchat);
            user.UpdateYoutube(request.Youtube);
            user.UpdateBlog(request.Blog);
            user.UpdateDateOfBirth(request.DateOfBirth);

            await _userRepository.UpdateUserAsync(user);

            _logger?.LogInformation($"{nameof(UpdateUserProfileCommand)}: done", request.UserId);

            return Unit.Value;
        }
    }
}

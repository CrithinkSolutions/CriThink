using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Providers.EmailSender.Services;
using MediatR;

namespace CriThink.Server.Application.CommandHandlers.Identity
{
    internal class CleanUpUsersScheduledDeletionCommandHandler : IRequestHandler<CleanUpUsersScheduledDeletionCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSenderService _emailSender;

        public CleanUpUsersScheduledDeletionCommandHandler(
            IUserRepository userRepository,
            IEmailSenderService emailSender)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _emailSender = emailSender ??
                throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task<Unit> Handle(CleanUpUsersScheduledDeletionCommand request, CancellationToken cancellationToken)
        {
            var deletedUsers = await DeleteAsync();

            foreach (var user in deletedUsers)
            {
                await _emailSender.SendAccountDeletionConfirmationEmailAsync(user.Email, user.UserName);
            }

            return Unit.Value;
        }

        private async Task<IList<User>> DeleteAsync()
        {
            var deletedUsers = await _userRepository.DeleteUserScheduledDeletionAsync();
            return deletedUsers;
        }
    }
}

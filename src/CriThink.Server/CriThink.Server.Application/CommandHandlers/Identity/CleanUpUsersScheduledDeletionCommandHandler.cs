using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Providers.EmailSender.Exceptions;
using CriThink.Server.Providers.EmailSender.Services;
using MediatR;

namespace CriThink.Server.Application.CommandHandlers.Identity
{
    internal class CleanUpUsersScheduledDeletionCommandHandler : IRequestHandler<CleanUpUsersScheduledDeletionCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSendingFailureRepository _emailSendingFailureRepository;
        private readonly IEmailSenderService _emailSender;

        public CleanUpUsersScheduledDeletionCommandHandler(
            IUserRepository userRepository,
            IEmailSendingFailureRepository emailSendingFailureRepository,
            IEmailSenderService emailSender)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _emailSender = emailSender ??
                throw new ArgumentNullException(nameof(emailSender));

            _emailSendingFailureRepository = emailSendingFailureRepository ??
                throw new ArgumentNullException(nameof(emailSendingFailureRepository));
        }

        public async Task<Unit> Handle(CleanUpUsersScheduledDeletionCommand request, CancellationToken cancellationToken)
        {
            var deletedUsers = await DeleteAsync();

            foreach (var user in deletedUsers)
            {
                await SendEmailAsync(user, cancellationToken);
            }

            return Unit.Value;
        }

        private async Task<IList<User>> DeleteAsync()
        {
            var deletedUsers = await _userRepository.DeleteUserScheduledDeletionAsync();
            return deletedUsers;
        }

        private async Task SendEmailAsync(
            User user,
            CancellationToken cancellationToken)
        {
            try
            {
                await _emailSender.SendAccountDeletionConfirmationEmailAsync(user.Email, user.UserName);
            }
            catch (CriThinkEmailSendingFailureException ex)
            {
                var failure = EmailSendingFailure.Create(
                    ex.FromAddress,
                    ex.Recipients,
                    ex.HtmlBody,
                    ex.Subject,
                    ex.Message);

                await _emailSendingFailureRepository.AddFailureAsync(failure, cancellationToken);
                await _emailSendingFailureRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
        }
    }
}

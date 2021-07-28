using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Providers.EmailSender.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Application.CommandHandlers.Identity
{
    internal class CleanUpUsersScheduledDeletionCommandHandler : IRequestHandler<CleanUpUsersScheduledDeletionCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly IEmailSenderService _emailSender;

        public CleanUpUsersScheduledDeletionCommandHandler(
            CriThinkDbContext dbContext,
            IEmailSenderService emailSender)
        {
            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));

            _emailSender = emailSender ??
                throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task<Unit> Handle(CleanUpUsersScheduledDeletionCommand request, CancellationToken cancellationToken)
        {
            var deletedUsers = await Delete(cancellationToken);

            foreach (var user in deletedUsers)
            {
                await _emailSender.SendAccountDeletionConfirmationEmailAsync(user.Email, user.UserName);
            }

            return Unit.Value;
        }

        private async Task<List<User>> Delete(CancellationToken cancellationToken)
        {
            const string sqlCommand = "DELETE FROM users\n" +
                                      "WHERE deletion_scheduled_on < now() AT TIME ZONE 'UTC'\n" +
                                      "RETURNING *";

            var deletedUsers = await _dbContext.Users
                .FromSqlRaw(sqlCommand)
                .AsNoTracking()
                .ToListAsync();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return deletedUsers;
        }
    }
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;

namespace CriThink.Server.Infrastructure.Repositories
{
    internal class EmailSendingFailureRepository : IEmailSendingFailureRepository
    {
        private readonly CriThinkDbContext _dbContext;

        public EmailSendingFailureRepository(
            CriThinkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public async Task<IList<EmailSendingFailure>> GetAllFailuresAsync(CancellationToken cancellationToken = default)
        {
            var collection = await _dbContext.EmailSendingFailures.GetAllEmailSendingFailuresAsync(cancellationToken);
            return collection;
        }

        public async Task AddFailureAsync(
            EmailSendingFailure failure,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.EmailSendingFailures.AddAsync(failure, cancellationToken);
        }

        public void RemoveFailure(EmailSendingFailure failure)
        {
            _dbContext.EmailSendingFailures.Remove(failure);
        }
    }
}

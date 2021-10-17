using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Domain.Repositories
{
    public interface IEmailSendingFailureRepository : IRepository<EmailSendingFailure>
    {
        Task<IList<EmailSendingFailure>> GetAllFailuresAsync(CancellationToken cancellationToken = default);

        Task AddFailureAsync(
            EmailSendingFailure failure,
            CancellationToken cancellationToken = default);

        void RemoveFailure(EmailSendingFailure failure);
    }
}

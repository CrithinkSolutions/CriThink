using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    public class GetAllSubscribedUsersHandler : IRequestHandler<GetAllSubscribedUsersCommand, IList<GetAllSubscribedUsersResponse>>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetAllSubscribedUsersHandler> _logger;

        public GetAllSubscribedUsersHandler(CriThinkDbContext dbContext, ILogger<GetAllSubscribedUsersHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public Task<IList<GetAllSubscribedUsersResponse>> Handle(GetAllSubscribedUsersCommand request, CancellationToken cancellationToken)
        {
            var users = _dbContext.UnknownSourceNotificationRequests.Where(usnr => usnr.UnknownNewsSourceId == request.UnknownNewsSourceId);

            var x = users.Select(u => new GetAllSubscribedUsersResponse
            {
                Email = u.Email,
                Id = u.Id,
            });

            return x;
        }
    }
}

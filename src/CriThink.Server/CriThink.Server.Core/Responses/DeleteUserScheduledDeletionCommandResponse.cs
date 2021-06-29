using System;
using System.Collections.Generic;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.Responses
{
    public class DeleteUserScheduledDeletionCommandResponse
    {
        public DeleteUserScheduledDeletionCommandResponse(IEnumerable<User> users)
        {
            if (users is null)
                throw new ArgumentNullException(nameof(User));

            Users = new List<User>(users);
        }

        public List<User> Users { get; }
    }
}

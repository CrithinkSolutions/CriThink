using System;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class UpdateUserCommand : IRequest
    {
        public UpdateUserCommand(Guid id,
            string userName,
            bool? isEmailConfirmed,
            bool? isLockoutEnabled,
            DateTime? lockoutEnd)
        {
            Id = id;
            UserName = userName;
            IsEmailConfirmed = isEmailConfirmed;
            IsLockoutEnabled = isLockoutEnabled;
            LockoutEnd = lockoutEnd;
        }

        public Guid Id { get; }

        public string UserName { get; }

        public bool? IsEmailConfirmed { get; }

        public bool? IsLockoutEnabled { get; }

        public DateTime? LockoutEnd { get; }
    }
}

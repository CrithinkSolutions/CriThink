using System;
using System.Collections.Generic;

namespace CriThink.Server.Application.Administration.ViewModels
{
    public class UserGetDetailsViewModel
    {
        public string UserId { get; set; }

        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public bool IsDeleted { get; set; }

        public int AccessFailedCount { get; set; }

        public bool IsLockoutEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public IReadOnlyCollection<string> Roles { get; set; }
    }
}

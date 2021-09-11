using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Application.Administration.ViewModels
{
    public class UserGetViewModel
    {
        public string UserId { get; set; }

        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public bool IsDeleted { get; set; }

        public IReadOnlyCollection<string> Roles { get; set; }
    }
}
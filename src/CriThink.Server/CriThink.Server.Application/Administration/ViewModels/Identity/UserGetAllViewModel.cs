using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Application.Administration.ViewModels
{
    public class UserGetAllViewModel
    {
        public UserGetAllViewModel(IEnumerable<UserGetViewModel> users, bool hasNextPage)
        {
            Users = new List<UserGetViewModel>(users);
            HasNextPage = hasNextPage;
        }

        public IReadOnlyList<UserGetViewModel> Users { get; }

        public bool HasNextPage { get; }
    }
}

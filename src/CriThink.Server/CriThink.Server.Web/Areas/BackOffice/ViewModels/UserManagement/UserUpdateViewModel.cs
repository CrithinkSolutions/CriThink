using System;
using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.UserManagement
{
    public class UserUpdateViewModel
    {
        [Required]
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public bool? IsEmailConfirmed { get; set; }

        public bool? IsLockoutEnabled { get; set; }

        public DateTime? LockoutEnd { get; set; }
    }
}
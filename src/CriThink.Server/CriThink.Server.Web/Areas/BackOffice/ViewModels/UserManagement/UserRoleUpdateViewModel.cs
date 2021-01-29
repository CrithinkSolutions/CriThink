using System;
using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.UserManagement
{
    public class UserRoleUpdateViewModel
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        public string Role { get; set; }
    }
}
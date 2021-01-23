using System;
using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.UserManagement
{
    public class SimpleUserManagementViewModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}
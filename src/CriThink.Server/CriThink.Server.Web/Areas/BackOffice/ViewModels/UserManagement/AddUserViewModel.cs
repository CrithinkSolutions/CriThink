using System.ComponentModel.DataAnnotations;
using CriThink.Common.Helpers;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.UserManagement
{
    public class AddUserViewModel
    {
        [Required]
        public string UserName { get; set; }
        
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[^\w\d\s:])([^\s])*$", ErrorMessage = "Password must have: one non-alphanumeric character, one digit (0-9), one uppercase (A-Z)")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public string Message { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

#pragma warning disable CA1056 // URI-like properties should not be strings
namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Email or Username")]
        [Required]
        public string EmailOrUsername { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public string ImagePath { get; set; }
    }
}

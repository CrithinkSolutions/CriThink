using System.ComponentModel.DataAnnotations;

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
    }
}

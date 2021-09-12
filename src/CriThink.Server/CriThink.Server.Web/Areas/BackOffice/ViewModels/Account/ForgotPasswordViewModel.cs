using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Display(Name = "Email or Username", Prompt = "Username or Email")]
        [Required(ErrorMessage = "Please insert email or username")]
        [StringLength(50)]
        public string EmailOrUsername { get; set; }

        public string Message { get; set; }
    }
}
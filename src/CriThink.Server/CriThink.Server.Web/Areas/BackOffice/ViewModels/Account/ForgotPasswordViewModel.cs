using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [MinLength(2)]
        [Required]
        public string EmailOrUsername { get; set; }

        public string Message { get; set; }
    }
}
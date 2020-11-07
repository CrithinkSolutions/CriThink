using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.Public.ViewModel.Identity
{
    public class ResetPasswordRequestViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Code { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

#pragma warning disable CA1056 // URI-like properties should not be strings
namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string EmailOrUsername { get; set; }

        public string Message { get; set; }
    }
}
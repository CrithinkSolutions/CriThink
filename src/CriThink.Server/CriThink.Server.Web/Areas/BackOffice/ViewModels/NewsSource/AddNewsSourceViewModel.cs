using System.ComponentModel.DataAnnotations;

#pragma warning disable CA1056 // URI-like properties should not be strings
namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource
{
    public class AddNewsSourceViewModel
    {
        [Required]
        public string Uri { get; set; }

        [Required]
        public Classification Classification { get; set; }

        public string Message { get; set; }
    }
}

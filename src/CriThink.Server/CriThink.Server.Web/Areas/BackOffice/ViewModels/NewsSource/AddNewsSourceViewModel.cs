using System.ComponentModel.DataAnnotations;

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

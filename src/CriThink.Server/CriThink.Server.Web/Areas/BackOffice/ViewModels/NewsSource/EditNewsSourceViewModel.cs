using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource
{
    public class EditNewsSourceViewModel
    {
        [Required]
        [MinLength(2)]
        public string OldLink { get; set; }

        [Required]
        [MinLength(2)]
        public string NewLink { get; set; }

        [Required]
        public NewsSourceAuthenticityViewModel Classification { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource
{
    public class EditNewsSourceViewModel
    {
        [Url]
        [Required]
        public string OldLink { get; set; }

        [Url]
        [Required]
        public string NewLink { get; set; }

        [Required]
        public Classification Classification { get; set; }
    }
}

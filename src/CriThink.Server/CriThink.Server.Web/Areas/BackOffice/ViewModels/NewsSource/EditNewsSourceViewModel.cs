using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource
{
    public class EditNewsSourceViewModel
    {
        [Required]
        public string OldLink { get; set; }

        [Required]
        public string NewLink { get; set; }

        [Required]
        public Classification Classification { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.Shared
{
    public class SimplePagification
    {
        [Required]
        public int? pageSize { get; set; } = 20;

        [Required]
        public int? pageIndex { get; set; } = 1;
    }
}
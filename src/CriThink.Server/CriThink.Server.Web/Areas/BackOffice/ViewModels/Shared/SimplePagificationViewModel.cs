using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.Shared
{
    public class SimplePagificationViewModel
    {
        public int? pageSize { get; set; } = 20;
        
        public int? pageIndex { get; set; } = 1;
    }
}
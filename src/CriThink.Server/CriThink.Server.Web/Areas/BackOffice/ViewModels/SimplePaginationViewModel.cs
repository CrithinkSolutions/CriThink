namespace CriThink.Server.Web.Areas.BackOffice.ViewModels
{
    public class SimplePaginationViewModel
    {
        public int PageSize { get; set; } = 20;

        public int PageIndex { get; set; }
    }
}
using System;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource
{
    public class UnknownNewsSourceViewModel
    {
        public Guid Id { get; set; }
        public string Uri { get; set; }
        public Classification Classification { get; set; }
    }
}

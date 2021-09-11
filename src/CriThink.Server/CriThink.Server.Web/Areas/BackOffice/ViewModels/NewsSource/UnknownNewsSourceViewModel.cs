using System;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource
{
    public class UnknownNewsSourceViewModel
    {
        public Guid Id { get; set; }

        public string Source { get; set; }

        public NewsSourceAuthenticityViewModel Classification { get; set; }
    }
}

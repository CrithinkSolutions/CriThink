using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.DebunkingNews
{
    public class UpdateDebunkingNewsViewModel
    {
        [Required]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Caption { get; set; }

        [Url]
        public string Link { get; set; }

        public IReadOnlyList<string> Keywords { get; set; }

    }
}
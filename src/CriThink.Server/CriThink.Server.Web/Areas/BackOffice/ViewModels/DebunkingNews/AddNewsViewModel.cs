using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.DebunkingNews
{
    public class AddNewsViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Caption { get; set; }

        [Required]
        [Url]
        public string Link { get; set; }

        [Required]
        [Url]
        public string ImageLink { get; set; }

        [Required]
        public IReadOnlyCollection<string> Keywords { get; set; }

        public string Message { get; set; }
    }
}
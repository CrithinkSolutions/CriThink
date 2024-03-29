using System;
using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.DebunkingNews
{
    public class UpdateDebunkingNewsViewModel
    {
        [Required]
        public Guid Id { get; set; }

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

        public string Keywords { get; set; }

        public string Message { get; set; }
    }
}
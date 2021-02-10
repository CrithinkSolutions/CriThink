using System;
using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.DebunkingNews
{
    public class SimpleDebunkingNewsViewModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}
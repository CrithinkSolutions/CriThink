using System;
using System.ComponentModel.DataAnnotations;

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.DebunkingNews
{
    public class SimpleDebunkingNewsViewModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}
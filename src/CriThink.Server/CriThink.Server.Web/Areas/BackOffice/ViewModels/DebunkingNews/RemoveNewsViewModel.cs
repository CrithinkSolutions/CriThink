using System;
using System.ComponentModel.DataAnnotations;

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.DebunkingNews
{
    public class RemoveNewsViewModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}
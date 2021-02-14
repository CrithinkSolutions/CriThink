using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriThink.Server.Core.Entities
{
    public class DebunkingNews : ICriThinkIdentity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public DateTime PublishingDate { get; set; }

        public string Link { get; set; }

        public string ImageLink { get; set; }

        [MaxLength(500)]
        public string NewsCaption { get; set; }

        public string Keywords { get; set; }

        [Required]
        public DebunkingNewsPublisher Publisher { get; set; }
    }
}

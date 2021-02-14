using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CA2227 // Collection properties should be read only
namespace CriThink.Server.Core.Entities
{
    public class DebunkingNewsPublisher : ICriThinkIdentity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Link { get; set; }

        public string Description { get; set; }

        public string Opinion { get; set; }

        public string FacebookPage { get; set; }

        public string InstagramProfile { get; set; }

        public string TwitterProfile { get; set; }

        #region Foreign Keys

        [Required]
        public DebunkingNewsLanguage Language { get; set; }

        [Required]
        public DebunkingNewsCountry Country { get; set; }

        public ICollection<DebunkingNews> DebunkingNews { get; set; }

        #endregion
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Core.Entities
{
    /// <summary>
    /// Contract for database entities
    /// </summary>
    public interface ICriThinkIdentity
    {
        [Key]
        public Guid Id { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriThink.Server.Domain.Entities
{
    /// <summary>
    /// Contract for database entities
    /// </summary>
    public abstract class Entity<TKey> : DomainEventsBasedObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; protected set; }
    }
}

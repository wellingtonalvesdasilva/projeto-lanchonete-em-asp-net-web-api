using System.ComponentModel.DataAnnotations;

namespace Arquitetura.Entity
{
    public abstract class EntityBase
    {
        [Key]
        public long Id { get; set; }
        public int Status { get; set; }
    }
}

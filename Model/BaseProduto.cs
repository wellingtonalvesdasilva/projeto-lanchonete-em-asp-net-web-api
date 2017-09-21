using Arquitetura.Entity;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class BaseProduto : EntityBase
    {
        [Required]
        public string Nome { get; set; }

        public decimal Valor { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Lanche : BaseProduto
    {
        [Required]
        public virtual ICollection<Porcao> Porcoes { get; set; }

        public Lanche()
        {
            this.Porcoes = new HashSet<Porcao>();
        }
    }
}

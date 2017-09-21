using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Porcao
    {
        public long Id { get; set; }

        public virtual Lanche Lanche { get; set; }

        public int Quantidade { get; set; }

        [Required(ErrorMessage = "Ingrediente é obrigatório")]
        [ForeignKey("Ingrediente")]
        public long IngredienteId { get; set; }

        public virtual Ingrediente Ingrediente { get; set; }
    }
}

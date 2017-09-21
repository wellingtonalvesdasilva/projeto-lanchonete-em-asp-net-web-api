using Arquitetura.Business;
using Model;
using Util;

namespace Business
{
    public class IngredienteBusiness : BusinessCrudGeneric<LanchoneteEntities, Ingrediente>
    {
        public Ingrediente BuscarIngrediente(Enumeracao.ETipoDeIngrediente tipoDeIngrediente)
        {
            return repository.GetById((long)tipoDeIngrediente);
        }
    }
}

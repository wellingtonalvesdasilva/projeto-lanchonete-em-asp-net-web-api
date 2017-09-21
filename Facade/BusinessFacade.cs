using Arquitetura.Facade;
using Business;
using Model;

namespace Facade
{
    public class BusinessFacade : BusinessFacadeBase<LanchoneteEntities>
    {
        private LancheBusiness lancheBusiness;
        public LancheBusiness Lanche
        {
            get { return lancheBusiness ?? (lancheBusiness = new LancheBusiness()); }
        }

        private IngredienteBusiness ingredienteBusiness;
        public IngredienteBusiness Ingrediente
        {
            get { return ingredienteBusiness ?? (ingredienteBusiness = new IngredienteBusiness()); }
        }

        public static BusinessFacade GetInstance()
        {
            return GetInstance<BusinessFacade>();
        }
    }
}

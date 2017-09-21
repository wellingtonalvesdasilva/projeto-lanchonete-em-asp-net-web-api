using Arquitetura.Facade;
using Arquitetura.Repository;
using Model;

namespace Facade
{
    public class RepositoryFacade: RepositoryFacadeBase<LanchoneteEntities>
    {
        //Poderia substitui isso por um método da super classe

        private GenericRepository<LanchoneteEntities, Lanche> lancheRepository;
        public GenericRepository<LanchoneteEntities, Lanche> Lanche
        {
            get { return lancheRepository ?? (lancheRepository = new GenericRepository<LanchoneteEntities, Lanche>()); }
        }

        private GenericRepository<LanchoneteEntities, Ingrediente> ingredienteRepository;
        public GenericRepository<LanchoneteEntities, Ingrediente> Ingrediente
        {
            get { return ingredienteRepository ?? (ingredienteRepository = new GenericRepository<LanchoneteEntities, Ingrediente>()); }
        }

        public static RepositoryFacade GetInstance()
        {
            return GetInstance<RepositoryFacade>();
        }
    }
}

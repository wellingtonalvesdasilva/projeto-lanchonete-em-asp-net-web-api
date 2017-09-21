using Arquitetura.Entity;
using Facade;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace Teste
{
    [TestClass]
    public class TestsBase
    {
        public BusinessFacade BusinessFacade;
        public RepositoryFacade RepositoryFacade;

        private FabricaDeDados fabricaDeDados;

        protected FabricaDeDados FabricaDeDados
        {
            get { return fabricaDeDados ?? (fabricaDeDados = new FabricaDeDados()); }
        }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            DbContextHelper<LanchoneteEntities>.CreateContext();

            FabricaDeDados.IncluirTodosIngredientesDisponiveis();

            BusinessFacade = new BusinessFacade(); //Aqui não pode ser singleton
            RepositoryFacade = new RepositoryFacade(); //Aqui não pode ser singleton
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            DbContextHelper<LanchoneteEntities>.DisposeContext();
        }
    }
}

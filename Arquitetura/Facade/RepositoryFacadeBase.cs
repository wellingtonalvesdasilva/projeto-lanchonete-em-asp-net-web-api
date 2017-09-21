using System.Data.Entity;
using System.Web;

namespace Arquitetura.Facade
{
    public abstract class RepositoryFacadeBase<TContext>
    where TContext : DbContext, new()
    {
        const string REPOSITORY_FACADE_KEY = "__RepositoryFacade";
        private static RepositoryFacadeBase<TContext> instanceCreatedForTDDOrConsoleApp = null;

        public static TAppRepositoryFacade GetInstance<TAppRepositoryFacade>() where TAppRepositoryFacade : RepositoryFacadeBase<TContext>, new()
        {
            if (UnitTestDetector.IsInUnitTest)
            {
                if (instanceCreatedForTDDOrConsoleApp == null)
                    instanceCreatedForTDDOrConsoleApp = new TAppRepositoryFacade();

                return (TAppRepositoryFacade)instanceCreatedForTDDOrConsoleApp;
            }
            else if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items[REPOSITORY_FACADE_KEY] == null)
                    HttpContext.Current.Items[REPOSITORY_FACADE_KEY] = new TAppRepositoryFacade();
                return (TAppRepositoryFacade)HttpContext.Current.Items[REPOSITORY_FACADE_KEY];
            }
            else
                return null;
        }

        protected static void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (UnitTestDetector.IsInUnitTest)
                    instanceCreatedForTDDOrConsoleApp = null;
                else if (HttpContext.Current != null && (HttpContext.Current.Items[REPOSITORY_FACADE_KEY] != null))
                    HttpContext.Current.Items[REPOSITORY_FACADE_KEY] = null;
            }
        }

        public static void Dispose()
        {
            Dispose(true);
        }
    }
}

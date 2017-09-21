using Arquitetura.Entity;
using System;
using System.Data.Entity;
using System.Web;

namespace Arquitetura.Facade
{
    public abstract class BusinessFacadeBase<TContext> : IDisposable
        where TContext : DbContext, new()
    {
        private TContext ctx;
        public TContext Context
        {
            get { return ctx ?? (ctx = DbContextHelper<TContext>.GetActiveContext()); }
            set { ctx = value; }
        }

        #region INSTANCE

        private static BusinessFacadeBase<TContext> instanceCreatedForTDDOrConsoleApp = null;

        private const string BUSINESS_FACADE_KEY = "__BusinessFacade";

        public static TAppBOFacade GetInstance<TAppBOFacade>() where TAppBOFacade : /*subclass of*/ BusinessFacadeBase<TContext>, new()
        {
            if (UnitTestDetector.IsInUnitTest)
            {
                if (instanceCreatedForTDDOrConsoleApp == null)
                    instanceCreatedForTDDOrConsoleApp = new TAppBOFacade();

                return (TAppBOFacade)instanceCreatedForTDDOrConsoleApp;
            }
            else if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items[BUSINESS_FACADE_KEY] == null)
                    HttpContext.Current.Items[BUSINESS_FACADE_KEY] = new TAppBOFacade();
                return (TAppBOFacade)HttpContext.Current.Items[BUSINESS_FACADE_KEY];
            }
            else
                return null;
        }

        #endregion

        #region DISPOSE

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (UnitTestDetector.IsInUnitTest)
                    instanceCreatedForTDDOrConsoleApp = null;
                else if (HttpContext.Current != null && (HttpContext.Current.Items[BUSINESS_FACADE_KEY] != null))
                    HttpContext.Current.Items[BUSINESS_FACADE_KEY] = null;
            }
        }

        #endregion
    }
}

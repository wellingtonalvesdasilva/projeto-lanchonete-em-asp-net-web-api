using Arquitetura.Facade;
using Effort;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Arquitetura.Entity
{
    public static class DbContextHelper<TContext> where TContext : DbContext, new()
    {
        public static TContext GetActiveContext()
        {
            var objCtxType = typeof(TContext);
            if (UnitTestDetector.IsInUnitTest)
                return DbContextHelper.GetDbContextForNonWebApp(objCtxType) as TContext;
            else
                return DbContextHelper.GetDbContextForWebApp(objCtxType) as TContext;
        }

        public static TContext CreateContext()
        {
            if (UnitTestDetector.IsInUnitTest)
                return DbContextHelper.CreateContextForNonWebApp<TContext>();
            else
                return DbContextHelper.CreateContextForWebApp<TContext>();
        }

        public static void DisposeContext()
        {
            if (!IsRequestingStaticContent())
            {
                DbContextHelper.DisposeContext(typeof(TContext));
                RepositoryFacadeBase<TContext>.Dispose();
            }
        }

        private static bool IsRequestingStaticContent()
        {
            if (HttpContext.Current == null)
                return false;
            string url = HttpContext.Current.Request.Url.ToString().ToLower();
            return (url.EndsWith(".css") || url.EndsWith(".js") || url.EndsWith(".png") || url.EndsWith(".jpg"));
        }
    }

    public static class DbContextHelper
    {
        internal const string ACTIVE_CONTEXT_KEY = "__ActiveDbContext";

        private static ObjectStorageManager<DbContext> objStorageMgr = new ObjectStorageManager<DbContext>();

        private static List<KeyValuePair<string, DbContext>> ctxForWebApp
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Items == null || HttpContext.Current.Items[ACTIVE_CONTEXT_KEY] == null)
                    return null;
                else
                    return (HttpContext.Current.Items[ACTIVE_CONTEXT_KEY] as List<KeyValuePair<string, DbContext>>);
            }
        }

        public static DbContext GetActiveContext()
        {
            if (UnitTestDetector.IsInUnitTest)
                if (objStorageMgr.CurrentThreadObjects.Count == 0)
                    return null;
                else
                    return objStorageMgr.CurrentThreadObjects[0].Value;
            else // aplicação web
                return GetDbContextForWebApp(0);
        }

        /// <summary>
        /// Obtem contexto ativo usando reflection + generic
        /// </summary>
        public static DbContext GetActiveContext(Type contextType)
        {
            Type contextHelperType = typeof(DbContextHelper<>);
            Type genericContextHelperType = contextHelperType.MakeGenericType(contextType);

            MethodInfo getActiveContextMethod = genericContextHelperType.GetMethod("GetActiveContext");

            return (DbContext)getActiveContextMethod.Invoke(null, null);
        }

        internal static DbContext GetDbContextForNonWebApp(Type dbContextType)
        {
            return objStorageMgr.GetObject(dbContextType.Name);
        }

        internal static DbContext GetDbContextForWebApp(int element)
        {
            if (ctxForWebApp == null)
                return null;
            else
                return ctxForWebApp[element].Value;
        }

        internal static DbContext GetDbContextForWebApp(Type DbContextType)
        {
            if (ctxForWebApp == null)
                return null;

            if (ctxForWebApp.Any(kv => kv.Key == DbContextType.Name))
                return ctxForWebApp.Single(kv => kv.Key == DbContextType.Name).Value;

            return null;
        }

        public static DbContext CreateContext(Type contextType)
        {
            Type contextHelperType = typeof(DbContextHelper<>);
            Type genericContextHelperType = contextHelperType.MakeGenericType(contextType);

            // evoca versão genérica que cria contexto para requisicao web ou teste/app console
            return (DbContext)genericContextHelperType.GetMethod("CreateContext").Invoke(null, null);
        }

        internal static TContext CreateContextForNonWebApp<TContext>(/*IDataLoader loader = null*/) where TContext : DbContext, new()
        {
            AssertContextIsNotCreatedYet(typeof(TContext));

            var ctxTypeName = typeof(TContext).Name;
            DbContext ctx;

            CheckAppConfigFileName();

            if (UnitTestDetector.IsInUnitTest)
                ctx = CreateContextWithInMemoryConnection<TContext>(/*loader*/);
            else
                ctx = new TContext();

            objStorageMgr.Add(ctxTypeName, ctx);

            return ctx as TContext;
        }

        private static void AssertContextIsNotCreatedYet(Type contextType)
        {
            if (GetActiveContext(contextType) != null)
                throw new InvalidOperationException("Context for " + contextType.Name + " already created; disposal should be done with DbContextHelper<TContext>.DisposeContext()");
        }

        private static void CheckAppConfigFileName()
        {
            var configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            if (configFile.ToLower().Contains("mstest.exe.config"))
                throw new InvalidOperationException(string.Format("Invalid Configuration File ({0}) - tests running from MSTest.exe shouldn't use the /noisolation command line flag", configFile));
        }

        private static TContext CreateContextWithInMemoryConnection<TContext>(/*IDataLoader loader = null*/ /*IDataLoader causa dependencia de Effort.dll em outros projetos que usam esta classe*/) where TContext : DbContext, new()
        {
            var entityConnectionString = "name=" + typeof(TContext).Name;
            var connectionString = ConfigurationManager.ConnectionStrings[typeof(TContext).Name].ConnectionString;

            // data source existe na conexão do code first
            if (connectionString.ToLower().Contains("data source"))
            {
                var connection = DbConnectionFactory.CreateTransient();
                return Activator.CreateInstance(typeof(TContext), connection) as TContext;
            }

            return Activator.CreateInstance(typeof(TContext), Effort.EntityConnectionFactory.CreateTransient(entityConnectionString), true) as TContext;
        }

        private static void InitializeCtxForWebApp()
        {
            HttpContext.Current.Items[ACTIVE_CONTEXT_KEY] = new List<KeyValuePair<string, DbContext>>();
        }

        internal static TContext CreateContextForWebApp<TContext>() where TContext : DbContext, new()
        {
            if (HttpContext.Current == null || HttpContext.Current.Items == null)
                throw new InvalidOperationException("No HttpContext available");

            AssertContextIsNotCreatedYet(typeof(TContext));

            var DbContextType = typeof(TContext);

            if (ctxForWebApp == null)
                InitializeCtxForWebApp();
            else
            {
                var ctx = GetDbContextForWebApp(DbContextType);
                if (ctx != null)
                    DisposeContext(DbContextType);
            }

            var key = DbContextType.Name;
            var value = new TContext();
            var newContext = new KeyValuePair<string, DbContext>(key, value);

            ctxForWebApp.Add(newContext);

            return value as TContext;
        }

        public static void DisposeContext(Type dbContextType)
        {
            if (UnitTestDetector.IsInUnitTest)
            {
                if (objStorageMgr.CurrentThreadObjects != null)
                {
                    var ctx = GetDbContextForNonWebApp(dbContextType);
                    if (ctx != null)
                        ctx.Dispose();

                    objStorageMgr.Remove(dbContextType.Name);
                }
            }
            else // aplicação web
            {
                var ctx = GetDbContextForWebApp(dbContextType);
                if (ctx != null)
                {
                    ctx.Dispose();
                    var ctxKV = ctxForWebApp.Single(c => c.Key == dbContextType.Name);
                    ctxForWebApp.Remove(ctxKV);
                }
            }
        }
    }
}

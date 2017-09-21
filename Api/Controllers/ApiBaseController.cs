using Arquitetura.Entity;
using Arquitetura.Repository;
using Facade;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace Api.Controllers
{
    public class ApiBaseController<TContext, TEntity> : ApiController
        where TContext : DbContext, new()
        where TEntity : class
    {
        public RepositoryFacade RepositoryFacade => RepositoryFacade.GetInstance();

        public BusinessFacade BusinessFacade => BusinessFacade.GetInstance();

        protected GenericRepository<TContext, TEntity> Repository;

        public ApiBaseController()
        {
            if (DbContextHelper<TContext>.GetActiveContext() == null)
                DbContextHelper<TContext>.CreateContext();

            this.Repository = new GenericRepository<TContext, TEntity>();
        }

        protected override void Dispose(bool disposing)
        {
            DbContextHelper<TContext>.DisposeContext();
            RepositoryFacade.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Consultar Todos
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get()
        {
            return this.Repository.GetAll().ToList();
        }

        /// <summary>
        /// Consultar por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public virtual TEntity Get(long id)
        {
            var entidade = this.Repository.GetById(id);
            return entidade;
        }
    }
}

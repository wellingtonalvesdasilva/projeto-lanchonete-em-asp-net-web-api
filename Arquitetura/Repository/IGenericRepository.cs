using System;
using System.Linq;
using System.Linq.Expressions;

namespace Arquitetura.Repository
{
    public interface IGenericRepository<TContext, TEntity> where TContext : class where TEntity : class
    {
        void Create(TEntity entity, bool saveChanges = true);

        void Update(TEntity entity, bool saveChanges = true);

        void Delete(TEntity entity, bool saveChanges = true);

        void Save(bool saveChanges = true);

        IQueryable<TEntity> GetAll();

        TEntity GetById(params object[] IDs);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool noTracking = false);

        TEntity GetUnique(Expression<Func<TEntity, bool>> predicate, bool noTracking = false);

        TEntity GetFirst(Expression<Func<TEntity, bool>> predicate);

        TContext Contexto { get; set; }
    }
}

using Arquitetura.Entity;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Arquitetura.Repository
{
    public class GenericRepository<TContext, TEntity> : IGenericRepository<TContext, TEntity>
        where TContext : DbContext, new()
        where TEntity : class
    {
        private TContext context;

        public GenericRepository() : this(DbContextHelper<TContext>.GetActiveContext() as TContext)
        {
        }

        public GenericRepository(TContext context)
        {
            this.context = context;
        }

        public TContext Contexto
        {
            get
            {
                return DbContextHelper<TContext>.GetActiveContext() as TContext;
            }
            set { this.context = value; }
        }

        public void Save(bool saveChanges = true)
        {
            if (saveChanges)
                context.SaveChanges();
        }

        private DbSet<TEntity> GetDbSet()
        {
            var fulltypename = typeof(TEntity).FullName;
            if (fulltypename == null)
                throw new ArgumentException("Tipo inválido para obter DbSet");
            return context.Set<TEntity>();
        }

        public virtual void Create(TEntity entity, bool saveChanges = true)
        {
            if (entity == null)
                throw new ArgumentException("Não pode adicionar entidade nula");

            GetDbSet().Add(entity);

            Save(saveChanges);
        }

        public virtual void Create(TEntity entity)
        {
            Create(entity, true);
        }

        public virtual void Update(TEntity entity, bool saveChanges = true)
        {
            Update(GetPrimaryKey(entity), entity, saveChanges);
        }

        public virtual void Update(long id, TEntity entity, bool saveChanges = true)
        {
            Update(new object[] { id }, entity, saveChanges);
        }

        protected virtual void Update(object[] keyValues, TEntity changedEntity, bool saveChanges = true)
        {
            TEntity originalEntity = (TEntity)GetById(keyValues);

            if (originalEntity == null)
                throw new ArgumentException("Não foi encontrada a chave primária " + keyValues.ToString());

            context.Entry(originalEntity).CurrentValues.SetValues(changedEntity);

            Save(saveChanges);
        }

        public virtual void Delete(TEntity entity, bool saveChanges = true)
        {
            if (entity == null)
                throw new ArgumentException("Não é possivel deletar uma entidade nula");

            GetDbSet().Remove(entity);

            Save(saveChanges);
        }

        public IQueryable<TEntity> GetAll()
        {
            return GetDbSet();
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> predicate)
        {
            var result = Find(predicate);
            if (result.Count() == 0)
                return null;
            else if (result.Count() > 1)
                throw new InvalidOperationException("Contém mais de um elemento");
            else
                return result.First();
        }

        public TEntity GetUnique(Expression<Func<TEntity, bool>> predicate, bool noTracking = false)
        {
            var result = Find(predicate, noTracking);
            if (result.Count() == 0)
                return null;

            return result.Take(1).FirstOrDefault();
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool noTracking = false)
        {
            if (noTracking)
                return GetDbSet().AsNoTracking().AsQueryable().Where(predicate);
            else
                return GetDbSet().AsQueryable().Where(predicate);
        }

        public TEntity GetById(params object[] IDs)
        {
            if (IDs[0] == null)
                return null;

            return GetDbSet().Find(IDs);
        }

        #region MÉTODOS AUXILIARES}

        protected object[] GetPrimaryKey(TEntity entity)
        {
            var keyNames = ObterChavesPrimariasPorNome();
            Type type = typeof(TEntity);

            object[] keyValues = new object[keyNames.Length];
            for (int i = 0; i < keyNames.Length; i++)
            {
                keyValues[i] = type.GetProperty(keyNames[i]).GetValue(entity, null);
            }

            return keyValues;
        }

        protected string[] ObterChavesPrimariasPorNome()
        {
            var internalObjectContext = ((IObjectContextAdapter)context).ObjectContext;
            var objectSet = internalObjectContext.CreateObjectSet<TEntity>();
            string[] keyNames = objectSet.EntitySet.ElementType.KeyMembers.Select(k => k.Name).ToArray();
            return keyNames;
        }

        #endregion
    }
}

using Microsoft.EntityFrameworkCore;
using Rise.PhoneBook.ApiCore.Core.Entities;
using System.Linq.Expressions;

namespace Rise.PhoneBook.ApiCore.Core
{
    public abstract class EfEntityRepositoryBase<TEntity, TContext>
          : IEntityRepository<TEntity>
          where TEntity : class, IEntity, new()
          where TContext : DbContext, new()
    {
        protected abstract List<TEntity> GetList(Expression<Func<TEntity,
            bool>> filter, TContext context);
         
        protected abstract TEntity Get(Expression<Func<TEntity, bool>> filter, TContext context);

        public TEntity Add(TEntity entity)
        {
            using (var context = new TContext())
            {
                var addedEntity = context.Entry(entity);

                addedEntity.State = EntityState.Added;

                context.SaveChanges();

                return addedEntity.Entity;
            }
        }
        public TEntity Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                var updatedEntity = context.Entry(entity);

                updatedEntity.State = EntityState.Modified;

                context.SaveChanges();

                return updatedEntity.Entity;
            }
        }

        public void Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;

                context.SaveChanges();
            }
        }
        public TEntity Get(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return Get(filter, context);
            }
        }

        public List<TEntity> GetEntities(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return GetList(filter, context);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

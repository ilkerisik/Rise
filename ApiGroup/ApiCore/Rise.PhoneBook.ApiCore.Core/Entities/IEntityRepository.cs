using System.Linq.Expressions;
namespace Rise.PhoneBook.ApiCore.Core.Entities
{
    public interface IEntityRepository<T> : IDisposable
    {
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
        T Get(Expression<Func<T, bool>> filter = null);
        List<T> GetEntities(Expression<Func<T, bool>> filter = null);
    }
}

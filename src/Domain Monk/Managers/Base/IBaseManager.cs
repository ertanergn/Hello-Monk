using System.Collections.Generic;

namespace Monk.Domain.Managers
{
    public interface IBaseManager<TEntity, TKey>
    {
        TEntity GetById(TKey id);
        TKey Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        IEnumerable<TEntity> GetAll();
    }
}
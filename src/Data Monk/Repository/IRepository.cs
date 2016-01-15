using System.Collections.Generic;

namespace Monk.Data.Repository
{
    public interface IRepository<TEntity,TKey>
    {
        /// <summary>
        /// Inserts entity to the database.
        /// </summary>
        TKey Insert(TEntity entity);

        /// <summary>
        /// Updates entity in the database.
        /// </summary>
        void Update(TEntity entity);

        /// <summary>
        /// Deletes an entity from database.
        /// </summary>
        void Delete(TEntity entity);

        /// <summary>
        /// Gets entity from the storage by it's Id.
        /// </summary>
        TEntity GetById(TKey id);

        /// <summary>
        /// Gets all entities of the type from the storage. 
        /// </summary>
        IList<TEntity> GetAll<TEntity>() where TEntity : class;
    }
}

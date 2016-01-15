using System;
using System.Collections.Generic;
using Monk.Core.Kernel;
using Monk.Data;
using Monk.Data.Exceptions;
using Monk.Data.Repository;
using Monk.Log;
using Ninject;

namespace Monk.Domain.Managers
{
    public class BaseManager<TEntity, TKey>: IBaseManager<TEntity, TKey> where TEntity : class
    {
        [Inject]
        public IRepository<TEntity,TKey> Repository { get; set; }

        [Inject]
        public ILog<BaseManager<TEntity,TKey>> Log { get; set; }

        public BaseManager()
        {
            ObjectFactory.ResolveDependencies(this);
        }

        /// <summary>
        /// Returns typeof TEntity type object by Id
        /// </summary>
        public virtual TEntity GetById(TKey id)
        {
            try
            {
                return Repository.GetById(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw new RepositoryException(string.Format("An error occurred while getting the entity {0} with id : {1}", typeof(TEntity).Name, id), ex);
            }
        }

        /// <summary>
        /// Insert typeof TEntity type object into database
        /// </summary>
        public virtual TKey Insert(TEntity entity)
        {
            try
            {
                return Repository.Insert(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw new RepositoryException(string.Format("An error occurred while inserting the entity {0}", typeof(TEntity).Name), ex);
            }
        }

        /// <summary>
        /// Updates TEntity type object in database
        /// </summary>
        public virtual void Update(TEntity entity)
        {
            try
            {
                Repository.Update(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw new RepositoryException(string.Format("An error occurred while updating the entity {0} with id: {1}", typeof(TEntity).Name, (entity as BaseEntity<TKey>).Id), ex);
            }
        }

        /// <summary>
        /// Deletes TEntity type object from database
        /// </summary>
        public virtual void Delete(TEntity entity)
        {
            try
            {
                Repository.Delete(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw new RepositoryException(string.Format("An error occurred while deleting the entity {0} with id: {1}", typeof(TEntity).Name, (entity as BaseEntity<TKey>).Id), ex);
            }
        }

        /// <summary>
        /// Returns all TEntity type objects
        /// </summary>
        public virtual IEnumerable<TEntity> GetAll()
        {
            try
            {
                return Repository.GetAll<TEntity>();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw new RepositoryException(string.Format("An error occurred while returning all records for {0}", typeof(TEntity).Name), ex);
            }          
        }
    }
}
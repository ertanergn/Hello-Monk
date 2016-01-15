using System.Collections.Generic;
using Monk.Core.Kernel;
using Monk.Data.Factories;
using Ninject;
using NHibernate;

namespace Monk.Data.Repository
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    {
        [Inject]
        public IRepositoryFactory SessionFactory { get; set; }

        ISessionFactory _sessionFactory { get; set; }

        public Repository()
        {
            ObjectFactory.ResolveDependencies(this);
            _sessionFactory = SessionFactory.GetSessionFactory();
        }

        /// <summary>
        /// Inserts a new entity
        /// </summary>
        public TKey Insert(TEntity entity)
        {
            TKey id;

            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(entity);
                    transaction.Commit();
                    id = (entity as BaseEntity<TKey>).Id;
                }
            }
            return id;
        }

        /// <summary>
        /// Updates given entity
        /// </summary>
        public void Update(TEntity entity)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(entity);
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Deletes selected entity
        /// </summary>
        public void Delete(TEntity entity)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(entity);
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Finds an entity with its Id and returns it
        /// </summary>
        public TEntity GetById(TKey id)
        {
            TEntity newEntity;

            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    newEntity = session.Get<TEntity>(id);
                    transaction.Commit();
                }
            }

            return newEntity;
        }

        /// <summary>
        /// Returns all entities for selected type
        /// </summary>
        public IList<TEntity> GetAll<TEntity>() where TEntity : class
        {
            IList<TEntity> listOfEntities;

            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    listOfEntities = session.CreateCriteria<TEntity>().List<TEntity>();
                    transaction.Commit();
                }
            }
            return listOfEntities;
        }
    }
}

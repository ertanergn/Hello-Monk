using NHibernate;
using NHibernate.Cfg;

namespace Monk.Data.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private ISessionFactory _sessionFactory;

        public RepositoryFactory()
        {
            InitializeRepository();
        }

        /// <summary>
        /// Returns initialized NHibernate session factory
        /// </summary>
        public ISessionFactory GetSessionFactory()
        {
            if(_sessionFactory == null)
                InitializeRepository();
            return _sessionFactory;
        }

        /// <summary>
        /// Initialize NHibernate configuration
        /// </summary>
        private void InitializeRepository()
        {
            Configuration cfg = new Configuration();
            cfg.Configure();
            _sessionFactory = cfg.BuildSessionFactory();
        }

        /// <summary>
        /// Dispose NHibernate SessionFactory
        /// </summary>
        public void Dispose()
        {
            if (_sessionFactory != null)
            {
                _sessionFactory.Dispose();
                _sessionFactory = null;
            }
        }
    }
}

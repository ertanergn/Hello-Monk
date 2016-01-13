using DataSessionFactory = NHibernate.ISessionFactory;
using NHibernate.Cfg;

namespace Monk.Data.Factories
{
    public class SessionFactory : ISessionFactory
    {
        private DataSessionFactory _sessionFactory;

        public SessionFactory()
        {
            InitializeRepository();
        }

        public DataSessionFactory GetSessionFactory()
        {
            return _sessionFactory;
        }

        /// <summary>
        /// Initialize NHibernate
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

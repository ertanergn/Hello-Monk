using NHibernate;

namespace Monk.Data.Factories
{
    public interface IRepositoryFactory
    {
        ISessionFactory GetSessionFactory();
    }
}

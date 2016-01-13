using DataSessionFactory = NHibernate.ISessionFactory;

namespace Monk.Data.Factories
{
    public interface ISessionFactory
    {
        DataSessionFactory GetSessionFactory();
    }
}

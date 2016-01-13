using Monk.Data.Factories;
using Ninject.Modules;

namespace Monk.Data
{
    public class DataModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISessionFactory>().To<SessionFactory>().InSingletonScope();
        }
    }
}

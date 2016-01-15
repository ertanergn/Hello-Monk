using Monk.Data.Factories;
using Ninject.Modules;

namespace Monk.Data
{
    public class DataModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepositoryFactory>().To<RepositoryFactory>().InSingletonScope();
        }
    }
}

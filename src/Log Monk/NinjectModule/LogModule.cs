using Ninject.Modules;

namespace Monk.Log
{
    public class LogModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(ILog<>)).To(typeof(Log4NetLogger<>)).InSingletonScope();
        }
    }
}

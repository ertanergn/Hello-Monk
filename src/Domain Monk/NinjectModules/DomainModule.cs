using System;
using Monk.Data.Repository;
using Monk.Domain.Entities;
using Monk.Domain.Managers.Implementation;
using Monk.Domain.Managers.Interface;
using Ninject.Modules;

namespace Monk.Domain.NinjectModules
{
    public class DomainModule : NinjectModule
    {
        public override void Load()
        {
            //Repository Bindings
            Bind<IRepository<Actor, Guid>>().To<Repository<Actor, Guid>>();
            Bind<IRepository<Message, Guid>>().To<Repository<Message, Guid>>();

            //Manager Bindings
            Bind<IActorManager>().To<ActorManager>();
            Bind<IMessageManager>().To<MessageManager>();
        }
    }
}

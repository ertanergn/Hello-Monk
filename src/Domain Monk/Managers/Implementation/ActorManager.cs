using System;
using Monk.Domain.Entities;
using Monk.Domain.Managers.Interface;
using Monk.Log;
using Ninject;

namespace Monk.Domain.Managers.Implementation
{
    public class ActorManager : BaseManager<Actor, Guid>, IActorManager
    {
        [Inject]
        public new ILog<ActorManager> Log { get; set; }

        /* Declare addtional methods apart from base */
    }
}

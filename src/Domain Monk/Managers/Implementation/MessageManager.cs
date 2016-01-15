using System;
using Monk.Domain.Entities;
using Monk.Domain.Managers.Interface;
using Monk.Log;
using Ninject;

namespace Monk.Domain.Managers.Implementation
{
    public class MessageManager : BaseManager<Message, Guid>, IMessageManager
    {
        [Inject]
        public new ILog<MessageManager> Log { get; set; } 

        /* Declare addtional methods apart from base */
    }
}

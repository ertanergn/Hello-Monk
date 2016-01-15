using System;
using Monk.Data;
using Monk.Domain.Enums;

namespace Monk.Domain.Entities
{
    /// <summary>
    /// This entity is used for storing successful/failed messages
    /// </summary>
    public class Message : BaseEntity<Guid>
    {
        public virtual string Name { get; set; }
        public virtual string Mail { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Content { get; set; }
        public virtual DateTime Time { get; set; }
        public virtual MessageStatus Status { get; set; }
    }
}

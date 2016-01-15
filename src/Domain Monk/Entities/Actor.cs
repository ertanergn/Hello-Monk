using System;
using Monk.Data;
using Monk.Domain.Enums;

namespace Monk.Domain.Entities
{
    /// <summary>
    /// This entity can be used later for keeping record of users
    /// </summary>
    public class Actor : BaseEntity<Guid>
    {
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Title { get; set; }
        public virtual DateTime DateOfBirth { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual string PicturePath { get; set; }
    }
}

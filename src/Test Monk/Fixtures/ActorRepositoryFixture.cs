using System;
using Monk.Core.Kernel;
using Monk.Data.Repository;
using Monk.Domain.Entities;
using Monk.Domain.Enums;
using Monk.Test.Base;
using NUnit.Framework;

namespace Monk.Test.Fixtures
{
    [TestFixture]
    public class ActorRepositoryFixture : BaseTestFixture
    {
        [Test]
        public void Insert_Fails_If_InsertMethodThrowsException()
        {
            var actorRepository = ObjectFactory.Get<IRepository<Actor, Guid>>();
            var actor = new Actor()
            {
                Name = "Test",
                Surname = "User",
                Gender = Gender.Male,
                DateOfBirth = DateTime.Now.AddYears(-30),
                Title = "Tester"
            };
            actorRepository.Insert(actor);
        }

        [Test]
        public void GetById_Fails_If_ReturnsNullOrThrowException()
        {
            var actorRepository = ObjectFactory.Get<IRepository<Actor, Guid>>();
            var actor = new Actor()
            {
                Name = "Test",
                Surname = "User",
                Gender = Gender.Male,
                DateOfBirth = DateTime.Now.AddYears(-30),
                Title = "Tester"
            };
            actorRepository.Insert(actor);
            Assert.AreNotEqual(actor.Id,Guid.Empty);

            var actorFromDb = actorRepository.GetById(actor.Id);
            Assert.NotNull(actorFromDb);
            Assert.AreEqual(actorFromDb.Id, actor.Id);         
        }

        [Test]
        public void GetBy_Fails_If_NotReturnNull()
        {
            var actorRepository = ObjectFactory.Get<IRepository<Actor, Guid>>();
            var actor = actorRepository.GetById(Guid.Empty);
            Assert.Null(actor);
        }
    }
}

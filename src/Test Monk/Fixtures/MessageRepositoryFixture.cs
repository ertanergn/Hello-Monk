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
    public class MessageRepositoryFixture : BaseTestFixture
    {
        [Test]
        public void Insert_Fails_If_InsertMethodThrowsException()
        {
            var messageRepository = ObjectFactory.Get<IRepository<Message, Guid>>();
            var message = new Message()
            {
                Name = "Test",
                Mail = "test@test.com",
                PhoneNumber = "555-555-555",
                Status = MessageStatus.Success,
                Content  = "Test Content",
                Time = DateTime.Now
            };

            messageRepository.Insert(message);
        }

        [Test]
        public void GetById_Fails_If_ReturnsNullOrThrowException()
        {
            var messageRepository = ObjectFactory.Get<IRepository<Message, Guid>>();
            var message = new Message()
            {
                Name = "Test",
                Mail = "test@test.com",
                PhoneNumber = "555-555-555",
                Status = MessageStatus.Success,
                Content = "Test Content",
                Time = DateTime.Now
            };
            messageRepository.Insert(message);
            Assert.AreNotEqual(message.Id, Guid.Empty);

            var messageFromDb = messageRepository.GetById(message.Id);
            Assert.NotNull(messageFromDb);
            Assert.AreEqual(messageFromDb.Id, message.Id);
        }

        [Test]
        public void GetBy_Fails_If_NotReturnNull()
        {
            var messageRepository = ObjectFactory.Get<IRepository<Message, Guid>>();
            var message =  messageRepository.GetById(Guid.Empty);
            Assert.Null(message);
        }
    }
}

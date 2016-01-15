using System;
using Monk.Core.Kernel;
using Monk.Domain.Entities;
using Monk.Domain.Enums;
using Monk.Domain.Managers.Interface;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Monk.Test.Base
{
    public class BaseTestFixture
    {
        [SetUp] //# Runs one time before starting test class
        public void SetupFixture()
        {
            BuildKernelForTest();
        }

        [TestFixtureSetUp] //# Runs every time before starting each test case
        public void Setup()
        {
        }

        [TearDown] //# Runs one time after finishing test class
        public void TearDownFixture()
        {
        }

        [TestFixtureTearDown] //# Runs every time after finishing each test case
        public void TearDown()
        {
            ObjectFactory.ResetKernel();
        }

        protected void CreateDatabaseSchema()
        {
            try
            {
                Configuration cfg = new Configuration();
                cfg.Configure();
                cfg.BuildSessionFactory();
                var schemaExport = new SchemaExport(cfg);
                schemaExport.Drop(false, true);
                schemaExport.Create(false, true);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot create database tables! more info" + ex.Message);
            }
        }

        protected void CreateDummyData()
        {
            CreateDummyActors();
            CreateDummyMessages();
        }

        #region Privates

        private void BuildKernelForTest()
        {
            ObjectFactory.BuildKernel();
        }

        private void CreateDummyMessages()
        {
            var messageManager = ObjectFactory.Get<IMessageManager>();

            for (int i = 1; i <= 10; i++)
            {
                var message = new Message()
                {
                    Mail = string.Format("TestUser{0}@test.com",i),
                    Name = "Hello Monk",
                    PhoneNumber = "5555-555-5555",
                    Status = i % 3 != 0 ? MessageStatus.Success : MessageStatus.Fail,
                    Content = "Hello",
                    Time = DateTime.Now.AddHours(-(new Random().Next(20, 30)))
                };
                messageManager.Insert(message);
            }
        }

        private void CreateDummyActors()
        {
            var actorManager = ObjectFactory.Get<IActorManager>();

            for (int i = 1; i <= 10; i++)
            {
                var actor = new Actor()
                {
                    Name = "TestUser_Name"+i,
                    Surname = "TEstUser_Surname"+i,
                    Gender = i%2 == 0 ? Gender.Male : Gender.Female,
                    Title = "Operator",
                    DateOfBirth = DateTime.Now.AddYears(-(new Random().Next(20,30)))
                };
                actorManager.Insert(actor);
            }
        }

        #endregion
    }
}

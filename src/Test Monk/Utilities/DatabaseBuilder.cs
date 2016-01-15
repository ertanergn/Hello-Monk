using Monk.Test.Base;
using NUnit.Framework;

namespace Monk.Test.Utilities
{
    [TestFixture]
    public class DatabaseBuilder : BaseTestFixture
    {
        [Test]
        public void CreateEmptyDatabase()
        {
            CreateDatabaseSchema();
        }

        [Test]
        public void CreateDummyContent()
        {
            CreateDummyData();
        }

        [Test]
        public void CreateEmptyDatabaseAndInsertDummyContent()
        {
            CreateDatabaseSchema();
            CreateDummyContent();
        }
    }
}

using MTA.Core.Application.Extensions;
using MTA.Core.Domain.Data.Helpers;
using MTA.UnitTests.TestModels;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Extensions
{
    [TestFixture]
    public class PropertyInfoExtensionsTests
    {
        [Test]
        public void FindPrimaryKeyProperty_PrimaryKeyExists_ReturnPrimaryKeyProperty()
        {
            var entity = new TestEntity()
            {
                Id = 1
            };

            var result = entity.GetPropertiesWithAttribute(typeof(ColumnAttribute)).FindPrimaryKeyProperty();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetValue(entity), Is.EqualTo(1));
        }

        [Test]
        public void FindPrimaryKeyProperty_PrimaryKeyNotExists_ReturnNull()
        {
            var entity = new TestEntityWithoutPrimaryKey()
            {
                Id = 1
            };

            var result = entity.GetPropertiesWithAttribute(typeof(ColumnAttribute)).FindPrimaryKeyProperty();

            Assert.That(result, Is.Null);
        }
    }
}
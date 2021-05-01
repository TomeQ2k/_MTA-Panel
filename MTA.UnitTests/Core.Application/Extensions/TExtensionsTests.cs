using System.Linq;
using MTA.Core.Application.Extensions;
using MTA.Core.Domain.Data.Helpers;
using MTA.UnitTests.TestModels;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Extensions
{
    [TestFixture]
    public class TExtensionsTests
    {
        [Test]
        public void GetPropertiesWithAttribute_WhenCalled_ReturnPropertiesWithGivenAttribute()
        {
            var entity = new TestEntity();

            var result = entity.GetPropertiesWithAttribute(typeof(ColumnAttribute));

            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(5));
        }
    }
}
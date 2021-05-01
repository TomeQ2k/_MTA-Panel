using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class BanServiceTests
    {
        private BanService banService;

        private Mock<IDatabase> database;
        private Mock<IHttpContextReader> httpContextReader;

        [SetUp]
        public void SetUp()
        {
            database = new Mock<IDatabase>();
            httpContextReader = new Mock<IHttpContextReader>();

            database.Setup(d => d.BanRepository.Insert(It.IsNotNull<Ban>(), It.IsAny<bool>()))
                .ReturnsAsync(true);

            banService = new BanService(database.Object, httpContextReader.Object);
        }

        #region AddBan

        [Test]
        public async Task AddBan_WhenCalled_ReturnBan()
        {
            var result = await banService.AddBan(new Ban());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Ban>());
        }

        [Test]
        public async Task AddBan_InsertingBanToDatabaseFailed_ReturnNull()
        {
            database.Setup(d => d.BanRepository.Insert(It.IsNotNull<Ban>(), It.IsAny<bool>()))
                .ReturnsAsync(false);

            var result = await banService.AddBan(new Ban());

            Assert.That(result, Is.Null);
        }

        #endregion
    }
}
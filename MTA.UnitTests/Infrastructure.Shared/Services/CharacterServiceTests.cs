using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class CharacterServiceTests
    {
        private CharacterService characterService;

        private Mock<IDatabase> database;
        private Mock<IHttpContextReader> httpContextReader;

        private const int UserId = 1;

        [SetUp]
        public void SetUp()
        {
            database = new Mock<IDatabase>();
            httpContextReader = new Mock<IHttpContextReader>();

            database.Setup(d => d.CharacterRepository.UpdateRange(It.IsNotNull<List<Character>>())).ReturnsAsync(true);
            database.Setup(d => d.EstateRepository.UpdateRange(It.IsNotNull<List<Estate>>())).ReturnsAsync(true);
            database.Setup(d => d.VehicleRepository.UpdateRange(It.IsNotNull<List<Vehicle>>())).ReturnsAsync(true);
            httpContextReader.Setup(h => h.CurrentUserId).Returns(UserId);

            characterService = new CharacterService(database.Object, httpContextReader.Object);
        }

        #region TransferMoney

        [Test]
        public void TransferMoney_SourceCharacterNotExist_ThrowEntityNotFoundException()
        {
            Assert.That(() => characterService.TransferMoney(null, new Character()),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void TransferMoney_TargetCharacterNotExist_ThrowEntityNotFoundException()
        {
            Assert.That(() => characterService.TransferMoney(new Character(), null),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void TransferMoney_BothCharactersNotExist_ThrowEntityNotFoundException()
        {
            Assert.That(() => characterService.TransferMoney(null, null),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public async Task TransferMoney_WhenCalled_ReturnAreCharactersUpdated()
        {
            var result = await characterService.TransferMoney(new Character(), new Character());

            Assert.That(result, Is.TypeOf<bool>());
        }

        #endregion

        #region TransferEstatesAndVehicles

        [Test]
        public async Task TransferEstatesAndVehicles_EstatesUpdatingFailed_ReturnFalse()
        {
            database.Setup(d => d.EstateRepository.UpdateRange(It.IsNotNull<List<Estate>>())).ReturnsAsync(false);

            var result = await characterService.TransferEstatesAndVehicles(new List<Estate>(), new List<Vehicle>(), 1);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TransferEstatesAndVehicles_VehiclesUpdatingFailed_ReturnFalse()
        {
            database.Setup(d => d.VehicleRepository.UpdateRange(It.IsNotNull<List<Vehicle>>())).ReturnsAsync(false);

            var result = await characterService.TransferEstatesAndVehicles(new List<Estate>(), new List<Vehicle>(), 1);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TransferEstatesAndVehicles_EstatesAndVehiclesUpdatingFailed_ReturnFalse()
        {
            database.Setup(d => d.EstateRepository.UpdateRange(It.IsNotNull<List<Estate>>())).ReturnsAsync(false);
            database.Setup(d => d.VehicleRepository.UpdateRange(It.IsNotNull<List<Vehicle>>())).ReturnsAsync(false);

            var result = await characterService.TransferEstatesAndVehicles(new List<Estate>(), new List<Vehicle>(), 1);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TransferEstatesAndVehicles_EstatesAndVehiclesWereUpdated_ReturnTrue()
        {
            var result = await characterService.TransferEstatesAndVehicles(new List<Estate>(), new List<Vehicle>(), 1);

            Assert.That(result, Is.True);
        }

        #endregion
    }
}
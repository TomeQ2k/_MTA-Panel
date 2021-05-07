using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
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
    public class CustomInteriorManagerTests
    {
        private CustomInteriorManager customInteriorManager;

        private Mock<IXmlReader> xmlReader;
        private Mock<IDatabase> database;
        private Mock<IFilesManager> filesManager;
        private Mock<IHttpContextReader> httpContextReader;

        private const int UserId = 1;

        private static Predicate<XElement> objectPredicate = (node) => node.Name.LocalName.Equals("object");
        private static Predicate<XElement> markerPredicate = (node) => node.Name.LocalName.Equals("marker");

        [SetUp]
        public void SetUp()
        {
            xmlReader = new Mock<IXmlReader>();
            filesManager = new Mock<IFilesManager>();
            database = new Mock<IDatabase>();
            httpContextReader = new Mock<IHttpContextReader>();

            xmlReader.Setup(x => x.GetDescendantNodes(It.IsNotNull<string>(), objectPredicate))
                .Returns(new List<XElement>() {new XElement("object")});
            xmlReader.Setup(x => x.GetDescendantNodes(It.IsNotNull<string>(), markerPredicate))
                .Returns(new List<XElement>() {new XElement("marker")});
            database.Setup(d => d.GameTempObjectRepository.InsertRange(It.IsAny<List<GameTempObject>>(), true))
                .ReturnsAsync(true);
            database.Setup(d => d.GameTempInteriorRepository.Insert(It.IsAny<GameTempInterior>(), false))
                .ReturnsAsync(true);
            httpContextReader.Setup(h => h.CurrentUserId).Returns(UserId);

            customInteriorManager = new CustomInteriorManager(xmlReader.Object, filesManager.Object, database.Object,
                httpContextReader.Object);
        }

        #region ExecuteAddCustomInterior

        [Test]
        public void ExecuteAddCustomInterior_InsertingTempObjectsToDatabaseFailed_DeleteFileShouldBeCalled()
        {
            database.Setup(d => d.GameTempObjectRepository.InsertRange(It.IsAny<List<GameTempObject>>(), true))
                .ReturnsAsync(false);

            customInteriorManager.ExecuteAddCustomInterior(new PremiumFile(),
                new List<GameTempObject>(), new GameTempInterior());

            filesManager.Verify(f => f.Delete(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ExecuteAddCustomInterior_InsertingTempObjectsToDatabaseFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.GameTempObjectRepository.InsertRange(It.IsAny<List<GameTempObject>>(), true))
                .ReturnsAsync(false);

            Assert.That(() => customInteriorManager.ExecuteAddCustomInterior(new PremiumFile(),
                new List<GameTempObject>(),
                new GameTempInterior()), Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public void ExecuteAddCustomInterior_InsertingTempInteriorsToDatabaseFailed_DeleteFileShouldBeCalled()
        {
            database.Setup(d => d.GameTempInteriorRepository.Insert(It.IsAny<GameTempInterior>(), false))
                .ReturnsAsync(false);

            customInteriorManager.ExecuteAddCustomInterior(new PremiumFile(),
                new List<GameTempObject>(), new GameTempInterior());

            filesManager.Verify(f => f.Delete(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ExecuteAddCustomInterior_InsertingTempInteriorsToDatabaseFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.GameTempInteriorRepository.Insert(It.IsAny<GameTempInterior>(), false))
                .ReturnsAsync(false);

            Assert.That(() => customInteriorManager.ExecuteAddCustomInterior(new PremiumFile(),
                new List<GameTempObject>(),
                new GameTempInterior()), Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task ExecuteAddCustomInterior_WhenCalled_VerifyDeleteFileNotCalled()
        {
            await customInteriorManager.ExecuteAddCustomInterior(new PremiumFile(), new List<GameTempObject>(),
                new GameTempInterior());

            filesManager.Verify(f => f.DeleteByFullPath(It.IsNotNull<string>()), Times.Never);
        }

        #endregion
    }
}
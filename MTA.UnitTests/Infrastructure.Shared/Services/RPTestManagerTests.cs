using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Persistence.Database;
using MTA.Infrastructure.Shared.Services;
using MTA.UnitTests.TestModels;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class RPTestManagerTests
    {
        private RPTestManager rpTestManager;

        private Mock<IDatabase> database;
        private Mock<IHttpContextReader> httpContextReader;
        private Mock<IRolesService> rolesService;

        private User user;
        private PassRPTestPartOneResult partOnePassResult;
        private Dictionary<int, int> partOneAnswersDictionary;
        private Dictionary<int, string> partTwoAnswersDictionary;
        private List<Question> partOneQuestions;
        private List<Question> partTwoQuestions;

        [SetUp]
        public void SetUp()
        {
            user = new User();
            var application = new Application();
            application.SetApplicant(user);

            partOnePassResult = new PassRPTestPartOneResult {IsPassed = true};

            partOneAnswersDictionary = new Dictionary<int, int>
            {
                {0, 0}
            };
            partTwoAnswersDictionary = new Dictionary<int, string>()
            {
                {1, "1"}, {2, "2"}, {3, "3"}, {4, "4"}
            };
            partOneQuestions = new List<Question>
            {
                new TestQuestion(0, "0", RPTestPartType.PartOne)
            };
            partTwoQuestions = new List<Question>
            {
                new TestQuestion(1, "1", RPTestPartType.PartTwo),
                new TestQuestion(2, "2", RPTestPartType.PartTwo),
                new TestQuestion(3, "3", RPTestPartType.PartTwo),
                new TestQuestion(4, "4", RPTestPartType.PartTwo)
            };

            database = new Mock<IDatabase>();
            httpContextReader = new Mock<IHttpContextReader>();
            rolesService = new Mock<IRolesService>();

            database.Setup(d => d.UserRepository.FindById(It.IsNotNull<int>()))
                .ReturnsAsync(user);
            database.Setup(d => d.UserRepository.Update(It.IsNotNull<User>())).ReturnsAsync(true);
            database.Setup(d => d.ApplicationRepository.Insert(It.IsNotNull<Application>(), It.IsNotNull<bool>()))
                .ReturnsAsync(true);
            database.Setup(d => d.BeginTransaction()).Returns(new DatabaseTransaction());
            database.Setup(d => d.ApplicationRepository.Update(It.IsNotNull<Application>())).ReturnsAsync(true);
            database.Setup(d => d.ApplicationRepository.GetApplicationWithApplicant(It.IsAny<int>()))
                .ReturnsAsync(application);
            rolesService.Setup(rs => rs.IsPermitted(It.IsAny<int>(), It.IsAny<RoleType[]>()))
                .ReturnsAsync(true);

            rpTestManager = new RPTestManager(database.Object, httpContextReader.Object, rolesService.Object);
        }

        #region PassRPTestPartOne

        [Test]
        public void PassRPTestPartOne_UserNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.UserRepository.FindById(It.IsNotNull<int>()))
                .ReturnsAsync(() => null);

            Assert.That(() => rpTestManager.PassRPTestPartOne(partOneAnswersDictionary),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void PassRPTestPartOne_TestIsAlreadyPassed_ThrowNoPermissionsException()
        {
            user.SetAppState(AppStateType.TestPassed);

            Assert.That(() => rpTestManager.PassRPTestPartOne(partOneAnswersDictionary),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public async Task PassRPTestPartOne_UserNotPassed_ReturnPassRPTestPartOneResultWithIsPassedFalse()
        {
            database.Setup(d => d.QuestionRepository.GetAll(It.IsNotNull<int>())).ReturnsAsync(partOneQuestions);

            partOneAnswersDictionary = new Dictionary<int, int>
            {
                {1, 1}
            };

            var result = await rpTestManager.PassRPTestPartOne(partOneAnswersDictionary);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PassRPTestPartOneResult>());
            Assert.That(result.IsPassed, Is.False);
        }

        [Test]
        public void PassRPTestPartOne_UserPassedAndUpdatingUserFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.QuestionRepository.GetAll(It.IsNotNull<int>())).ReturnsAsync(partOneQuestions);
            database.Setup(d => d.UserRepository.Update(It.IsNotNull<User>())).ReturnsAsync(false);

            Assert.That(() => rpTestManager.PassRPTestPartOne(partOneAnswersDictionary),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task PassRPTestPartOne_UserPassed_ReturnPassRPTestPartOneResultWithIsPassedTrue()
        {
            database.Setup(d => d.QuestionRepository.GetAll(It.IsNotNull<int>())).ReturnsAsync(partOneQuestions);

            var result = await rpTestManager.PassRPTestPartOne(partOneAnswersDictionary);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PassRPTestPartOneResult>());
            Assert.That(result.IsPassed, Is.True);
        }

        #endregion

        #region GenerateAnswersForPartTwo

        [Test]
        public void GenerateAnswersForPartTwo_UserNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.UserRepository.FindById(It.IsNotNull<int>()))
                .ReturnsAsync(() => null);

            Assert.That(() => rpTestManager.GenerateAnswersForPartTwo(partTwoAnswersDictionary),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        [TestCase(AppStateType.Accepted)]
        [TestCase(AppStateType.All)]
        public void GenerateAnswersForPartTwo_TestIsAlreadyAcceptedOrNotPassed_ThrowNoPermissionsException(
            AppStateType appStateType)
        {
            user.SetAppState(appStateType);

            Assert.That(() => rpTestManager.GenerateAnswersForPartTwo(partTwoAnswersDictionary),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void GenerateAnswersForPartTwo_InsertingApplicationFailed_ThrowDatabaseException()
        {
            user.SetAppState(AppStateType.TestPassed);
            database.Setup(d => d.QuestionRepository.GetAll(It.IsNotNull<int>())).ReturnsAsync(partTwoQuestions);
            database.Setup(d => d.ApplicationRepository.Insert(It.IsNotNull<Application>(), It.IsNotNull<bool>()))
                .ReturnsAsync(false);

            Assert.That(() => rpTestManager.GenerateAnswersForPartTwo(partTwoAnswersDictionary),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public void GenerateAnswersForPartTwo_UpdatingUserFailed_ThrowDatabaseException()
        {
            user.SetAppState(AppStateType.TestPassed);
            database.Setup(d => d.QuestionRepository.GetAll(It.IsNotNull<int>())).ReturnsAsync(partTwoQuestions);
            database.Setup(d => d.UserRepository.Update(It.IsNotNull<User>()))
                .ReturnsAsync(false);

            Assert.That(() => rpTestManager.GenerateAnswersForPartTwo(partTwoAnswersDictionary),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task GenerateAnswersForPartTwo_WhenCalled_ReturnTrue()
        {
            user.SetAppState(AppStateType.TestPassed);
            database.Setup(d => d.QuestionRepository.GetAll(It.IsNotNull<int>())).ReturnsAsync(partTwoQuestions);

            var result = await rpTestManager.GenerateAnswersForPartTwo(partTwoAnswersDictionary);

            Assert.That(result, Is.True);
        }

        #endregion

        #region ReviewRPTest

        [Test]
        public void ReviewRPTest_CurrentUserIsNotPermitted_ThrowNoPermissionsException()
        {
            rolesService.Setup(rs => rs.IsPermitted(It.IsAny<int>(), It.IsAny<RoleType[]>()))
                .ReturnsAsync(false);

            Assert.That(
                () => rpTestManager.ReviewRPTest(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ApplicationStateType>()),
                Throws.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void ReviewRPTest_ApplicationNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.ApplicationRepository.GetApplicationWithApplicant(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            Assert.That(
                () => rpTestManager.ReviewRPTest(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ApplicationStateType>()),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void ReviewRPTest_UpdateApplicationFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.ApplicationRepository.Update(It.IsNotNull<Application>())).ReturnsAsync(false);
            Assert.That(
                () => rpTestManager.ReviewRPTest(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ApplicationStateType>()),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public void ReviewRPTest_UpdaingUserFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.UserRepository.Update(It.IsNotNull<User>())).ReturnsAsync(false);
            Assert.That(
                () => rpTestManager.ReviewRPTest(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ApplicationStateType>()),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task ReviewRPTest_WhenCalled_ReturnTrue()
        {
            var result = await rpTestManager.ReviewRPTest(It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<ApplicationStateType>());

            Assert.That(result, Is.TypeOf<ReviewTestResult>());
        }

        #endregion
    }
}
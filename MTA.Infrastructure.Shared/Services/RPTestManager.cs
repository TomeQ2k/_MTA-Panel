using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class RPTestManager : IRPTestManager
    {
        private readonly IDatabase database;
        private readonly IHttpContextReader httpContextReader;
        private readonly IRolesService rolesService;

        private const int PartOneQuestionsCount = 8;
        private const int PartTwoQuestionsCount = 4;

        public RPTestManager(IDatabase database, IHttpContextReader httpContextReader, IRolesService rolesService)
        {
            this.database = database;
            this.httpContextReader = httpContextReader;
            this.rolesService = rolesService;
        }

        public async Task<IEnumerable<Question>> GetPartQuestions(RPTestPartType partType)
            => (await database.QuestionRepository.GetAll())
                .Where(q => q.Part == (int) partType)
                .SortRandom()
                .Take(partType == RPTestPartType.PartOne ? PartOneQuestionsCount : PartTwoQuestionsCount);

        public async Task<PassRPTestPartOneResult> PassRPTestPartOne(Dictionary<int, int> answersDictionary)
        {
            var user = await database.UserRepository.FindById(httpContextReader.CurrentUserId) ??
                       throw new EntityNotFoundException("User not found");

            if (user.IsAppStateGreaterThan(AppStateType.All))
                throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            var questions = await GetPartQuestions(RPTestPartType.PartOne);

            int points = 0;

            foreach (var key in answersDictionary.Keys)
            {
                if (questions.Any(q => q.Id == key && q.Key == answersDictionary[key]))
                    points++;
            }

            if ((double) points / answersDictionary.Count >= Constants.RPTestPassLimit)
            {
                user.SetAppState(AppStateType.TestPassed);

                return await database.UserRepository.Update(user)
                    ? new PassRPTestPartOneResult {IsPassed = true}
                    : throw new DatabaseException();
            }

            return new PassRPTestPartOneResult {IsPassed = false};
        }

        public async Task<bool> GenerateAnswersForPartTwo(Dictionary<int, string> answersDictionary)
        {
            var user = await database.UserRepository.FindById(httpContextReader.CurrentUserId) ??
                       throw new EntityNotFoundException("User not found");

            if (user.IsAppStateGreaterThan(AppStateType.Sent) || user.IsAppStateLowerThan(AppStateType.TestPassed))
                throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            var application = Application.Create(httpContextReader.CurrentUserId);

            var questions = await GetPartQuestions(RPTestPartType.PartTwo);

            var questionsAnswers = new List<(string, string)>();

            foreach (var key in answersDictionary.Keys)
            {
                var question = questions.FirstOrDefault(q => q.Id == key);
                questionsAnswers.Add((question?.Content, answersDictionary[key]));
            }

            bool isSucceeded;

            using (var transaction = database.BeginTransaction().Transaction)
            {
                application.SetQuestionsAndAnswers(questionsAnswers);

                user.SetAppState(AppStateType.Sent);

                isSucceeded = await database.ApplicationRepository.Insert(application) &&
                              await database.UserRepository.Update(user);

                transaction.Complete();
            }

            return isSucceeded ? true : throw new DatabaseException();
        }

        public async Task<ReviewTestResult> ReviewRPTest(int applicationId, string note, ApplicationStateType stateType)
        {
            if (!await rolesService.IsPermitted(httpContextReader.CurrentUserId, Constants.AdminsAndSupportersRoles))
                throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            var application = await database.ApplicationRepository.GetApplicationWithApplicant(applicationId)
                              ?? throw new EntityNotFoundException("Application not found");

            bool isSucceeded;

            using (var transaction = database.BeginTransaction().Transaction)
            {
                application.SetReviewDetails(httpContextReader.CurrentUserId, note, stateType);
                application.Applicant.SetAppState(stateType == ApplicationStateType.Passed
                    ? AppStateType.Accepted
                    : AppStateType.TestPassed);

                isSucceeded = await database.ApplicationRepository.Update(application) &&
                              await database.UserRepository.Update(application.Applicant);

                transaction.Complete();
            }

            return isSucceeded
                ? new ReviewTestResult(isSucceeded, application.ApplicantId)
                : throw new DatabaseException();
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Services
{
    public interface IRPTestManager : IReadOnlyRPTestManager
    {
        Task<PassRPTestPartOneResult> PassRPTestPartOne(Dictionary<int, int> answersDictionary);

        Task<bool> GenerateAnswersForPartTwo(Dictionary<int, string> answersDictionary);

        Task<ReviewTestResult> ReviewRPTest(int applicationId, string note, ApplicationStateType stateType);
    }
}
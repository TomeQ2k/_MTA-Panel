using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.UnitTests.TestModels
{
    public class TestQuestion : Question
    {
        public TestQuestion(int id, string content, RPTestPartType partType)
            => (Id, Content, Part) = (id, content, (int) partType);
    }
}
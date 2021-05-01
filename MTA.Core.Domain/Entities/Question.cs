using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class Question : EntityModel
    {
        [Column("id", true)] public int Id { get; protected set; }
        [Column("question")] public string Content { get; protected set; }
        [Column("answer1")] public string Answer1 { get; protected set; }
        [Column("answer2")] public string Answer2 { get; protected set; }
        [Column("answer3")] public string Answer3 { get; protected set; }
        [Column("key")] public int Key { get; protected set; }
        [Column("part")] public int Part { get; protected set; } = (int) RPTestPartType.PartOne;
    }
}
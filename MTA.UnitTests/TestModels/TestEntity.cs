using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Entities;

namespace MTA.UnitTests.TestModels
{
    public class TestEntity : EntityModel
    {
        [Column("id", true)] public int Id { get; set; }
        [Column("charactername")] public string Name { get; set; }
        [Column("money")] public long Money { get; set; }
        [Column("account")] public int AccountId { get; set; }

        [Column("IGNORED")] public bool Ignored { get; set; }
    }

    public class TestEntityWithoutPrimaryKey : EntityModel
    {
        [Column("id")] public int Id { get; set; }
        [Column("charactername")] public string Name { get; set; }
        [Column("money")] public long Money { get; set; }
        [Column("account")] public int AccountId { get; set; }
    }
}
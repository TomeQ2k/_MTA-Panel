using System.Linq;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class Faction : EntityModel
    {
        [Column("id", true)] public int Id { get; protected set; }
        [Column("name")] public string Name { get; protected set; }
        [Column("bankbalance")] public long BankBalance { get; protected set; }

        [Column("WorkersCount", customProperty: true)]
        public int WorkersCount { get; protected set; }

        #region ranks

        [Column("rank_1")] public string Rank1 { get; protected set; }
        [Column("rank_2")] public string Rank2 { get; protected set; }
        [Column("rank_3")] public string Rank3 { get; protected set; }
        [Column("rank_4")] public string Rank4 { get; protected set; }
        [Column("rank_5")] public string Rank5 { get; protected set; }
        [Column("rank_6")] public string Rank6 { get; protected set; }
        [Column("rank_7")] public string Rank7 { get; protected set; }
        [Column("rank_8")] public string Rank8 { get; protected set; }
        [Column("rank_9")] public string Rank9 { get; protected set; }
        [Column("rank_10")] public string Rank10 { get; protected set; }
        [Column("rank_11")] public string Rank11 { get; protected set; }
        [Column("rank_12")] public string Rank12 { get; protected set; }
        [Column("rank_13")] public string Rank13 { get; protected set; }
        [Column("rank_14")] public string Rank14 { get; protected set; }
        [Column("rank_15")] public string Rank15 { get; protected set; }
        [Column("rank_16")] public string Rank16 { get; protected set; }
        [Column("rank_17")] public string Rank17 { get; protected set; }
        [Column("rank_18")] public string Rank18 { get; protected set; }
        [Column("rank_19")] public string Rank19 { get; protected set; }
        [Column("rank_20")] public string Rank20 { get; protected set; }

        #endregion

        public string GetFactionRankName(int factionRank)
            => (string) GetType().GetProperties()
                .FirstOrDefault(p => p.GetCustomAttributes(false)
                    .OfType<ColumnAttribute>().FirstOrDefault(a => a.Name.Equals($"rank_{factionRank}")) != null)
                .GetValue(this);
    }
}
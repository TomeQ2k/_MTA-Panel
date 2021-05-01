using System;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class MtaLog : EntityModel
    {
        [Column("time")]public DateTime Time { get; protected set; }
        [Column("action")]public int Action { get; protected set; }
        [Column("source")]public string Source { get; protected set; }
        [Column("affected")]public string Affected { get; protected set; }
        [Column("content")]public string Content { get; protected set; }
    }
}
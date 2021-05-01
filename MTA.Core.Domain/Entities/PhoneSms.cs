using System;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class PhoneSms : EntityModel
    {
        [Column("Id", true)] public int Id { get; protected set; }
        [Column("date")] public DateTime Date { get; protected set; }
        [Column("action")] public int Action { get; protected set; }
        [Column("from")] public string From { get; protected set; }
        [Column("to")] public string To { get; protected set; }
        [Column("content")] public string Content { get; protected set; }
    }
}
using System;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class OrderTransaction : EntityModel
    {
        [Column("id", true)] public int Id { get; protected set; }
        [Column("transaction_id")] public string TransactionId { get; protected set; }
        [Column("donator_email")] public string Email { get; protected set; }
        [Column("amount")] public decimal Amount { get; protected set; }
        [Column("dt")] public DateTime DateCreated { get; protected set; } = DateTime.Now;
        [Column("username")] public string Username { get; protected set; }
        [Column("validated")] public int Validated { get; protected set; }

        public void SetTransactionId(string transactionId) => TransactionId = transactionId;
        public void SetAmount(decimal amount) => Amount = amount;
        public void SetEmail(string email) => Email = email;
        public void SetUsername(string username) => Username = username;

        public void Validate() => Validated = 1;

        public bool IsValidated => Validated == 1;
    }
}
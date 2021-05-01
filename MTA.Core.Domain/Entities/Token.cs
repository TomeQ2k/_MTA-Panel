using System;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class Token : EntityModel
    {
        [Column("id", true)] public string Id { get; protected set; } = Utils.Id();
        [Column("code")] public string Code { get; protected set; } = Utils.Token(32);
        [Column("dateCreated")] public DateTime DateCreated { get; protected set; } = DateTime.Now;
        [Column("tokenType")] public int TokenType { get; protected set; }
        [Column("userId")] public int UserId { get; protected set; }

        public User User { get; protected set; }

        public static Token Create(TokenType tokenType, int userId) =>
            new Token {TokenType = (int) tokenType, UserId = userId};

        public Token SetCode(string code)
        {
            Code = code;
            return this;
        }

        public void SetUser(User user)
            => User = user;
    }
}
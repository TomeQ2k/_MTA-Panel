using System;
using MTA.Core.Domain.Entities;

namespace MTA.UnitTests.TestModels
{
    public class TestToken : Token
    {
        public Token ChangeCreationDate(int addDays)
        {
            DateCreated = DateTime.Now.AddDays(addDays);
            return this;
        }
    }
}
using MTA.Core.Domain.Entities;

namespace MTA.UnitTests.TestModels
{
    public class DevUser : User
    {
        public DevUser()
        {
            Id = -1;
        }
    }
}
using MTA.Core.Domain.Entities;

namespace MTA.UnitTests.TestModels
{
    public class TestCharacter : Character
    {
        public TestCharacter SetEstate(Estate estate)
        {
            Estates.Add(estate);
            return this;
        }

        public TestCharacter SetVehicle(Vehicle vehicle)
        {
            Vehicles.Add(vehicle);
            return this;
        }
    }
}
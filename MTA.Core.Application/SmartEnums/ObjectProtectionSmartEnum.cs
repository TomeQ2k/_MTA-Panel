using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.SmartEnum;
using MTA.Core.Application.Results;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.SmartEnums
{
    public abstract class ObjectProtectionSmartEnum : SmartEnum<ObjectProtectionSmartEnum>
    {
        protected ObjectProtectionSmartEnum(string name, int value) : base(name, value)
        {
        }

        public static ObjectProtectionSmartEnum Estate = new EstateType();
        public static ObjectProtectionSmartEnum Vehicle = new VehicleType();

        public abstract Task<ObjectProtectionResult> ProtectObject(int objectId, int days,
            IEnumerable<Character> characters,
            IDatabase database);

        private sealed class EstateType : ObjectProtectionSmartEnum
        {
            public EstateType() : base(nameof(Estate), (int) ObjectProtectionType.Estate)
            {
            }

            public override async Task<ObjectProtectionResult> ProtectObject(int objectId, int days,
                IEnumerable<Character> characters,
                IDatabase database)
            {
                foreach (var character in characters)
                {
                    var estate = character.Estates.FirstOrDefault(e => e.Id == objectId);

                    if (estate != null)
                    {
                        estate.Protect(days);

                        return await database.EstateRepository.Update(estate)
                            ? new ObjectProtectionResult(true, ObjectProtectionType.Estate, objectId,
                                estate.ProtectedUntil)
                            : new ObjectProtectionResult(false, ObjectProtectionType.Estate, objectId);
                    }
                }

                return new ObjectProtectionResult(false, ObjectProtectionType.Estate, objectId);
            }
        }

        private sealed class VehicleType : ObjectProtectionSmartEnum
        {
            public VehicleType() : base(nameof(Vehicle), (int) ObjectProtectionType.Vehicle)
            {
            }

            public override async Task<ObjectProtectionResult> ProtectObject(int objectId, int days,
                IEnumerable<Character> characters,
                IDatabase database)
            {
                foreach (var character in characters)
                {
                    var vehicle = character.Vehicles.FirstOrDefault(v => v.Id == objectId);

                    if (vehicle != null)
                    {
                        vehicle.Protect(days);

                        return await database.VehicleRepository.Update(vehicle)
                            ? new ObjectProtectionResult(true, ObjectProtectionType.Vehicle, objectId,
                                vehicle.ProtectedUntil)
                            : new ObjectProtectionResult(false, ObjectProtectionType.Vehicle, objectId);
                    }
                }

                return new ObjectProtectionResult(false, ObjectProtectionType.Vehicle, objectId);
            }
        }
    }
}
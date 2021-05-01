using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class VehiclesShop : EntityModel
    {
        [Column("vehbrand")] public string VehicleBrand { get; protected set; }
        [Column("vehmodel")] public string VehicleModel { get; protected set; }
        [Column("vehyear")] public int VehicleYear { get; protected set; }
    }
}
using SmartRide.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Lookup;

public class VehicleType : LookupEntity
{
    [Key]
    [Column(TypeName = "int")]
    public new VehicleTypeEnum Id { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = [];
}

using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Lookup;

public class VehicleType : LookupEntity
{
    [Key]
    [Column(TypeName = "TINYINT")]
    public new VehicleTypeEnum Id { get; set; }

    [Required]
    [Column(TypeName = "TINYINT")]
    public int Capacity { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = [];
}

using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Entities.Lookup;
using SmartRide.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities;

[Index(nameof(Vin), IsUnique = true)]
[Index(nameof(Plate), IsUnique = true)]
public class Vehicle : BaseEntity
{
    [Required]
    [Column(TypeName = "char", Order = 0)]
    [StringLength(36)]
    public required string UserId { get; set; }

    [Required]
    [Column(TypeName = "int", Order = 1)]
    public required VehicleTypeEnum VehicleTypeId { get; set; }


    [Required]
    [Column(TypeName = "char")]
    [StringLength(17)]
    public required string Vin { get; set; }

    [Required]
    [Column(TypeName = "varchar")]
    [StringLength(10)]
    public required string Plate { get; set; }

    [Required]
    [Column(TypeName = "varchar")]
    [StringLength(50)]
    public required string Make { get; set; }

    [Required]
    [Column(TypeName = "varchar")]
    [StringLength(50)]
    public required string Model { get; set; }

    [Required]
    [Column(TypeName = "int")]
    public required int Year { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public required DateTime RegisteredDate { get; set; }

    [Required]
    [ForeignKey(nameof(UserId))]    // can be removed if the property name is UserId
    public required User User { get; set; }

    [Required]
    [ForeignKey(nameof(VehicleTypeId))]
    public required VehicleType VehicleType { get; set; }
}

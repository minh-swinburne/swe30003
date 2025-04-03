using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities;

public class Location : BaseEntity
{
    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(255)]
    public required string Address { get; set; }

    [Column(TypeName = "DOUBLE")]
    public double? Latitude { get; set; }

    [Column(TypeName = "DOUBLE")]
    public double? Longitude { get; set; }

    [Column(TypeName = "BINARY(16)")]
    public Guid? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}

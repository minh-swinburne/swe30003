using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Lookup;

public abstract class LookupEntity
{
    [Key]
    [Column(TypeName = "INT")]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public required string Name { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(255)]
    public string? Description { get; set; }
}

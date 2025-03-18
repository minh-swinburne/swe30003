using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Lookup;

public abstract class LookupEntity
{
    [Key]
    [Column(TypeName = "int")]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "varchar")]
    [StringLength(50)]
    public required string Name { get; set; }

    [Column(TypeName = "varchar")]
    [StringLength(255)]
    public string? Description { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    [Column(TypeName = "BINARY(16)")]
    public required Guid Id { get; set; }

    [Column(TypeName = "TIMESTAMP")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "TIMESTAMP")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedTime { get; set; } = DateTime.UtcNow;

    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    public virtual Guid GetId()
    {
        return Id;
    }

    public void UpdateTimestamp()
    {
        UpdatedTime = DateTime.UtcNow;
    }
}

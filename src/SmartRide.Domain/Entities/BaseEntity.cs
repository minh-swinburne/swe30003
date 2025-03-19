using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    [Column(TypeName = "binary")]
    [StringLength(36)]
    public required Guid Id { get; set; }

    [Column(TypeName = "timestamp")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "timestamp")]
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

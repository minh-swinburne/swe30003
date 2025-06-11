using SmartRide.Domain.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Base;

public class Feedback : BaseEntity
{
    [Required]
    [Column(TypeName = "BINARY(16)")]
    public Guid RideId { get; set; }

    [Required]
    [Range(1, 5)]
    [Column(TypeName = "TINYINT")]
    public int Rating { get; set; }

    [Column(TypeName = "TEXT")]
    public string? Comment { get; set; }

    [Required]
    [ForeignKey(nameof(RideId))]
    public Ride Ride { get; set; } = null!;

    public override void OnSave(string state)
    {
        base.OnSave(state);

        if (state == "Added")
            AddDomainEvent(new FeedbackCreatedEvent(this));
        else if (state == "Modified")
            AddDomainEvent(new FeedbackUpdatedEvent(this));
        else if (state == "Deleted")
            AddDomainEvent(new FeedbackDeletedEvent(this));
    }
}

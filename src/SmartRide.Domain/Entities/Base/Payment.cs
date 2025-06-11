using SmartRide.Common.Constants;
using SmartRide.Domain.Entities.Lookup;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Base;

public class Payment : BaseEntity
{
    [Required]
    [Column(TypeName = "BINARY(16)")]
    public Guid RideId { get; set; }

    [Required]
    [Range((double)PaymentConstants.MinAmount, (double)PaymentConstants.MaxAmount)]
    [Column(TypeName = "DECIMAL(20,4)")]
    public decimal Amount { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(3)]
    public string Currency { get; set; } = PaymentCurrencyEnum.USD.ToString();

    [Required]
    [Column(TypeName = "TINYINT")]
    public PaymentMethodEnum PaymentMethodId { get; set; }

    [Required]
    [Column(TypeName = "TINYINT")]
    public PaymentStatusEnum Status { get; set; } = PaymentStatusEnum.Pending;

    [Column(TypeName = "DATETIME")]
    public DateTime? TransactionTime { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(PaymentConstants.TransactionIdMaxLength)]
    public string? TransactionId { get; set; }

    [Required]
    [ForeignKey(nameof(RideId))]
    public Ride Ride { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(PaymentMethodId))]
    public PaymentMethod PaymentMethod { get; set; } = null!;

    public override void OnSave(string state)
    {
        base.OnSave(state);

        if (state == "Added")
            AddDomainEvent(new PaymentCreatedEvent(this));
        else if (state == "Modified")
            AddDomainEvent(new PaymentUpdatedEvent(this));
        else if (state == "Deleted")
            AddDomainEvent(new PaymentDeletedEvent(this));
    }
}

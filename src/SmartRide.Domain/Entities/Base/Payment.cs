﻿using Microsoft.EntityFrameworkCore;
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
    [Range(0, (double)decimal.MaxValue)]
    [Column(TypeName = "DECIMAL(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    [Column(TypeName = "TINYINT")]
    public PaymentMethodEnum PaymentMethodId { get; set; }

    [Required]
    [Column(TypeName = "DATETIME")]
    public DateTime PaymentTime { get; set; }

    [Column(TypeName = "TINYINT")]
    public PaymentStatusEnum Status { get; set; } = PaymentStatusEnum.Pending;

    [ForeignKey(nameof(RideId))]
    public Ride Ride { get; set; } = null!;

    [ForeignKey(nameof(PaymentMethodId))]
    public PaymentMethod PaymentMethod { get; set; } = null!;

    public override void OnSave(EntityState state)
    {
        base.OnSave(state);

        if (state == EntityState.Added)
        {
            AddDomainEvent(new PaymentCreatedEvent(this));
        }
        else if (state == EntityState.Modified)
        {
            AddDomainEvent(new PaymentUpdatedEvent(this));
        }
        else if (state == EntityState.Deleted)
        {
            AddDomainEvent(new PaymentDeletedEvent(this));
        }
    }
}

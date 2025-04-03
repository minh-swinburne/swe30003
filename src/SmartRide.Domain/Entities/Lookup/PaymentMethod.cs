using SmartRide.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Lookup;

public class PaymentMethod : LookupEntity
{
    [Key]
    [Column(TypeName = "TINYINT")]
    public new PaymentMethodEnum Id { get; set; }
}

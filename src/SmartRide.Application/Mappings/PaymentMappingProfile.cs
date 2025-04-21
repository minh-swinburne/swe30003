using AutoMapper;
using SmartRide.Application.Commands.Payments;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Application.Mappings;

public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        CreateMap<CreatePaymentCommand, Payment>();

        // Only map non-null properties and ensure strings are not whitespace
        CreateMap<UpdatePaymentCommand, Payment>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                srcMember != null &&
                (srcMember is not string str || !string.IsNullOrWhiteSpace(str)) &&
                (!srcMember.GetType().IsValueType || Nullable.GetUnderlyingType(srcMember.GetType()) != null)
            ));

        // Common mapping for BasePaymentResponseDTO
        CreateMap<Payment, BasePaymentResponseDTO>()
            .Include<Payment, CreatePaymentResponseDTO>()
            .Include<Payment, UpdatePaymentResponseDTO>()
            .Include<Payment, GetPaymentResponseDTO>()
            .Include<Payment, ListPaymentResponseDTO>()
            .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id));

        // Derived DTOs inherit the mapping from BasePaymentResponseDTO
        CreateMap<Payment, CreatePaymentResponseDTO>();
        CreateMap<Payment, UpdatePaymentResponseDTO>();
        CreateMap<Payment, GetPaymentResponseDTO>();
        CreateMap<Payment, ListPaymentResponseDTO>();
    }
}

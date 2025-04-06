using FluentValidation;
using SmartRide.Application.Commands.Locations;
using SmartRide.Common.Constants;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Validators.Locations;

public class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationCommandValidator()
    {
        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage(LocationErrors.ADDRESS_EMPTY.Message)
            .WithErrorCode(LocationErrors.ADDRESS_EMPTY.Code)
            .MaximumLength(LocationConstants.AddressMaxLength)
            .WithMessage(LocationErrors.ADDRESS_TOO_LONG.Message)
            .WithErrorCode(LocationErrors.ADDRESS_TOO_LONG.Code);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage(LocationErrors.LATITUDE_INVALID.Message)
            .WithErrorCode(LocationErrors.LATITUDE_INVALID.Code)
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage(LocationErrors.LONGITUDE_INVALID.Message)
            .WithErrorCode(LocationErrors.LONGITUDE_INVALID.Code)
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x.UserId)
            .Must(userId => userId == null || Guid.TryParse(userId.ToString(), out _))
            .WithMessage(LocationErrors.USER_ID_INVALID.Message)
            .WithErrorCode(LocationErrors.USER_ID_INVALID.Code);
    }
}

using FluentValidation;
using SmartRide.Common.Constants;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Commands.Users.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(UserErrors.FIRSTNAME_EMPTY.Message)
            .WithErrorCode(UserErrors.FIRSTNAME_EMPTY.Code)
            .MaximumLength(FieldLengths.NameMaxLength)
            .WithMessage(UserErrors.FIRSTNAME_TOO_LONG.Message)
            .WithErrorCode(UserErrors.FIRSTNAME_TOO_LONG.Code)
            .Matches(NamePattern.Regex)
            .WithMessage(UserErrors.FIRSTNAME_INVALID.Message)
            .WithErrorCode(UserErrors.FIRSTNAME_INVALID.Code);

        RuleFor(x => x.LastName)
            .MaximumLength(FieldLengths.NameMaxLength)
            .WithMessage(UserErrors.LASTNAME_TOO_LONG.Message)
            .WithErrorCode(UserErrors.LASTNAME_TOO_LONG.Code)
            .Matches(NamePattern.Regex)
            .WithMessage(UserErrors.LASTNAME_INVALID.Message)
            .WithErrorCode(UserErrors.LASTNAME_INVALID.Code)
            .When(x => !string.IsNullOrWhiteSpace(x.LastName)); // Only validate if LastName is not null or empty

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(UserErrors.EMAIL_EMPTY.Message)
            .WithErrorCode(UserErrors.EMAIL_EMPTY.Code)
            .MaximumLength(FieldLengths.EmailMaxLength)
            .WithMessage(UserErrors.EMAIL_TOO_LONG.Message)
            .WithErrorCode(UserErrors.EMAIL_TOO_LONG.Code)
            .Matches(EmailPattern.Regex)
            .WithMessage(UserErrors.EMAIL_INVALID.Message)
            .WithErrorCode(UserErrors.EMAIL_INVALID.Code);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage(UserErrors.PHONE_EMPTY.Message)
            .WithErrorCode(UserErrors.PHONE_EMPTY.Code)
            .MaximumLength(FieldLengths.PhoneMaxLength)
            .WithMessage(UserErrors.PHONE_TOO_LONG.Message)
            .WithErrorCode(UserErrors.PHONE_TOO_LONG.Code)
            .Matches(PhonePattern.Regex)
            .WithMessage(UserErrors.PHONE_INVALID.Message)
            .WithErrorCode(UserErrors.PHONE_INVALID.Code);

        RuleFor(x => x.Password)
            .MinimumLength(FieldLengths.PasswordMinLength)
            .WithMessage(UserErrors.PASSWORD_TOO_SHORT.Message)
            .WithErrorCode(UserErrors.PASSWORD_TOO_SHORT.Code)
            .MaximumLength(FieldLengths.PasswordMaxLength)
            .WithMessage(UserErrors.PASSWORD_TOO_LONG.Message)
            .WithErrorCode(UserErrors.PASSWORD_TOO_LONG.Code)
            .Matches(PasswordPattern.Regex)
            .WithMessage(UserErrors.PASSWORD_INVALID.Message)
            .WithErrorCode(UserErrors.PASSWORD_INVALID.Code)
            .When(x => !string.IsNullOrWhiteSpace(x.Password));
    }
}

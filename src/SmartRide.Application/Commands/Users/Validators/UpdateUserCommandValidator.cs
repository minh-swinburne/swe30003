using FluentValidation;
using SmartRide.Common.Constants;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Commands.Users.Validators;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(UserErrors.USER_ID_EMPTY.Message)
            .WithErrorCode(UserErrors.USER_ID_EMPTY.Code);

        RuleFor(x => x.FirstName)
            .MaximumLength(FieldLengths.NameMaxLength)
            .WithMessage(UserErrors.FIRSTNAME_TOO_LONG.Message)
            .WithErrorCode(UserErrors.FIRSTNAME_TOO_LONG.Code)
            .Matches(NamePattern.Regex)
            .WithMessage(UserErrors.FIRSTNAME_INVALID.Message)
            .WithErrorCode(UserErrors.FIRSTNAME_INVALID.Code)
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

        RuleFor(x => x.LastName)
            .MaximumLength(FieldLengths.NameMaxLength)
            .WithMessage(UserErrors.LASTNAME_TOO_LONG.Message)
            .WithErrorCode(UserErrors.LASTNAME_TOO_LONG.Code)
            .Matches(NamePattern.Regex)
            .WithMessage(UserErrors.LASTNAME_INVALID.Message)
            .WithErrorCode(UserErrors.LASTNAME_INVALID.Code)
            .When(x => !string.IsNullOrWhiteSpace(x.LastName));

        RuleFor(x => x.Email)
            .MaximumLength(FieldLengths.EmailMaxLength)
            .WithMessage(UserErrors.EMAIL_TOO_LONG.Message)
            .WithErrorCode(UserErrors.EMAIL_TOO_LONG.Code)
            .Matches(EmailPattern.Regex)
            .WithMessage(UserErrors.EMAIL_INVALID.Message)
            .WithErrorCode(UserErrors.EMAIL_INVALID.Code)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Phone)
            .MaximumLength(FieldLengths.PhoneMaxLength)
            .WithMessage(UserErrors.PHONE_TOO_LONG.Message)
            .WithErrorCode(UserErrors.PHONE_TOO_LONG.Code)
            .Matches(PhonePattern.Regex)
            .WithMessage(UserErrors.PHONE_INVALID.Message)
            .WithErrorCode(UserErrors.PHONE_INVALID.Code)
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

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

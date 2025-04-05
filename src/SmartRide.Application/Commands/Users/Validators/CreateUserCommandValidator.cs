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
            .WithMessage(CreateUserError.ERROR_001A.Message)
            .WithErrorCode(CreateUserError.ERROR_001A.Code)
            .MaximumLength(FieldLengths.NameMaxLength)
            .WithMessage(CreateUserError.ERROR_001B.Message)
            .WithErrorCode(CreateUserError.ERROR_001B.Code)
            .Matches(NamePattern.Regex)
            .WithMessage(CreateUserError.ERROR_001C.Message)
            .WithErrorCode(CreateUserError.ERROR_001C.Code);

        RuleFor(x => x.LastName)
            .MaximumLength(FieldLengths.NameMaxLength)
            .WithMessage(CreateUserError.ERROR_002A.Message)
            .WithErrorCode(CreateUserError.ERROR_002A.Code)
            .Matches(NamePattern.Regex)
            .WithMessage(CreateUserError.ERROR_002B.Message)
            .WithErrorCode(CreateUserError.ERROR_002B.Code);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(CreateUserError.ERROR_003A.Message)
            .WithErrorCode(CreateUserError.ERROR_003A.Code)
            .MaximumLength(FieldLengths.EmailMaxLength)
            .WithMessage(CreateUserError.ERROR_003B.Message)
            .WithErrorCode(CreateUserError.ERROR_003B.Code)
            .Matches(EmailPattern.Regex)
            .WithMessage(CreateUserError.ERROR_003C.Message)
            .WithErrorCode(CreateUserError.ERROR_003C.Code);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage(CreateUserError.ERROR_004A.Message)
            .WithErrorCode(CreateUserError.ERROR_004A.Code)
            .MaximumLength(FieldLengths.PhoneMaxLength)
            .WithMessage(CreateUserError.ERROR_004B.Message)
            .WithErrorCode(CreateUserError.ERROR_004B.Code)
            .Matches(PhonePattern.Regex)
            .WithMessage(CreateUserError.ERROR_004C.Message)
            .WithErrorCode(CreateUserError.ERROR_004C.Code);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(CreateUserError.ERROR_005A.Message)
            .WithErrorCode(CreateUserError.ERROR_005A.Code)
            .MinimumLength(FieldLengths.PasswordMinLength)
            .WithMessage(CreateUserError.ERROR_005B.Message)
            .WithErrorCode(CreateUserError.ERROR_005B.Code)
            .MaximumLength(FieldLengths.PasswordMaxLength)
            .WithMessage(CreateUserError.ERROR_005C.Message)
            .WithErrorCode(CreateUserError.ERROR_005C.Code)
            .Matches(PasswordPattern.Regex)
            .WithMessage(CreateUserError.ERROR_005D.Message)
            .WithErrorCode(CreateUserError.ERROR_005D.Code);
    }
}

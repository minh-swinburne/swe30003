using FluentValidation;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Commands.Users.Validators;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
  public DeleteUserCommandValidator()
  {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(UserErrors.USER_ID_EMPTY.Message)
            .WithErrorCode(UserErrors.USER_ID_EMPTY.Code);
        // .Must(userId => Guid.TryParse(userId.ToString(), out _))
        // .WithMessage(UserErrors.USER_ID_INVALID.Message)
        // .WithErrorCode(UserErrors.USER_ID_INVALID.Code);
  }
}

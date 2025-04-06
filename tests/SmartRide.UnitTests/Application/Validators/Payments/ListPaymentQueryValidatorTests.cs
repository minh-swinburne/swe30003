using FluentValidation.TestHelper;
using SmartRide.Application.Queries.Payments;
using SmartRide.Application.Validators.Payments;

namespace SmartRide.UnitTests.Application.Validators.Payments;

public class ListPaymentQueryValidatorTests
{
    private readonly ListPaymentQueryValidator _validator;

    public ListPaymentQueryValidatorTests()
    {
        _validator = new ListPaymentQueryValidator();
    }

    [Fact]
    public void Should_Have_Error_When_PageSize_Is_Invalid()
    {
        var query = new ListPaymentQuery { PageSize = 0 };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void Should_Have_Error_When_PageNo_Is_Invalid()
    {
        var query = new ListPaymentQuery { PageNo = 0 };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.PageNo);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Query_Is_Valid()
    {
        var query = new ListPaymentQuery { PageSize = 10, PageNo = 1 };
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveAnyValidationErrors();
    }
}

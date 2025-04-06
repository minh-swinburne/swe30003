using FluentValidation.TestHelper;
using SmartRide.Application.Queries.Locations;
using SmartRide.Application.Validators.Locations;

namespace SmartRide.UnitTests.Application.Validators.Locations;

public class ListLocationQueryValidatorTests
{
    private readonly ListLocationQueryValidator _validator;

    public ListLocationQueryValidatorTests()
    {
        _validator = new ListLocationQueryValidator();
    }

    [Fact]
    public void Should_Have_Error_When_PageSize_Is_Invalid()
    {
        var query = new ListLocationQuery { PageSize = 0 };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void Should_Have_Error_When_PageNo_Is_Invalid()
    {
        var query = new ListLocationQuery { PageNo = 0 };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.PageNo);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Query_Is_Valid()
    {
        var query = new ListLocationQuery
        {
            PageSize = 10,
            PageNo = 1,
            OrderBy = "Address"
        };
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveAnyValidationErrors();
    }
}

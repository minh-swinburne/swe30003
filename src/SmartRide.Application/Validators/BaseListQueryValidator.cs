using FluentValidation;
using Humanizer;
using SmartRide.Common.Helpers;
using SmartRide.Common.Interfaces;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using System.Reflection;

namespace SmartRide.Application.Validators;

public abstract class BaseListQueryValidator<TQuery, TEntity> : AbstractValidator<TQuery>
    where TQuery : ISortable, IPageable
    where TEntity : BaseEntity
{
    protected BaseListQueryValidator()
    {
        RuleFor(query => query.OrderBy)
            .Must(orderBy => string.IsNullOrEmpty(orderBy) || QueryHelper.GetProperty<TEntity>(orderBy) != null)
            .WithMessage(QueryErrors.ORDERBY_INVALID.Message)
            .WithErrorCode(QueryErrors.ORDERBY_INVALID.Code);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage(QueryErrors.PAGE_SIZE_INVALID.Message)
            .WithErrorCode(QueryErrors.PAGE_SIZE_INVALID.Code);

        RuleFor(x => x.PageNo)
            .GreaterThan(0)
            .WithMessage(QueryErrors.PAGE_NO_INVALID.Message)
            .WithErrorCode(QueryErrors.PAGE_NO_INVALID.Code);
    }
}

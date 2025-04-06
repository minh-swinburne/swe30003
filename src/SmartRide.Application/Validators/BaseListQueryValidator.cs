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
            .WithMessage(QueryErrors.INVALID_ORDERBY.Message)
            .WithErrorCode(QueryErrors.INVALID_ORDERBY.Code);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage(QueryErrors.INVALID_PAGE_SIZE.Message)
            .WithErrorCode(QueryErrors.INVALID_PAGE_SIZE.Code);

        RuleFor(x => x.PageNo)
            .GreaterThan(0)
            .WithMessage(QueryErrors.INVALID_PAGE_NO.Message)
            .WithErrorCode(QueryErrors.INVALID_PAGE_NO.Code);
    }
}

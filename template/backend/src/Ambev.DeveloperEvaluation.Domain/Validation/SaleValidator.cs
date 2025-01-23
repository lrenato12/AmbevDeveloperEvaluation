using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(x => x.Number)
            .NotNull()
            .NotEqual(0)
            .WithMessage("Sale number must be provided");

        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("Branch cannot be empty");

        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer cannot be empty");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("The list of items cannot be empty");
    }
}
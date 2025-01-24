using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("Branch cannot be empty");

        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer's Sale cannot be empty");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("List of items cannot be empty");

        RuleForEach(x => x.Items)
            .SetValidator(new CreateSaleItemValidator());
    }
}

public class CreateSaleItemValidator : AbstractValidator<CreateSaleItemRequest>
{
    public CreateSaleItemValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product cannot be empty");

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("Quantity cannot be empty");
    }
}
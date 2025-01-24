using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name of the product cannot be empty");

        RuleFor(x => x.UnitPrice)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Unit price must be greather than zero");
    }
}
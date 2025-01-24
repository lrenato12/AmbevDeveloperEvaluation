using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductCommand : IRequest<CreateProductResult>
{
    public string Name { get; set; }
    public decimal UnitPrice { get; set; }

    public CreateProductCommand(string name, decimal unitPrice)
    {
        Name = name;
        UnitPrice = unitPrice;
    }
}
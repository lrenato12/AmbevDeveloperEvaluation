using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSales;

/// <summary>
/// Represents the command to create a sale on the database
/// </summary>
public class CreateSaleCommand : IRequest<CreateSaleResult>
{
    /// <summary>
    /// Gets the id of the branch where the sale are created
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets the id from the customer who created the sale.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets the list of the items on the sale 
    /// </summary>
    public List<CreateSaleItemCommand> Items { get; set; } = new();
}

/// <summary>
/// Represent a item of the sale with product and quantity
/// </summary>
public class CreateSaleItemCommand
{
    /// <summary>
    /// Id of the product
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Quantity of the product ordered
    /// </summary>
    public int Quantity { get; set; } = 0;
}
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Represents the command to update a sale on the database
/// </summary>
public class UpdateSaleCommand : IRequest<UpdateSaleResult>
{
    /// <summary>
    /// Gets the id of the sale
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets the list of the items on the sale 
    /// </summary>
    public List<UpdateSaleItemCommand> Items { get; set; } = new();

    /// <summary>
    /// Flag that represents that the sale should be canceled
    /// </summary>
    public bool Cancel { get; set; }
}

/// <summary>
/// Represent a item of the sale with product and quantity
/// </summary>
public class UpdateSaleItemCommand
{
    /// <summary>
    /// Id of the product
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Quantity of the product ordered
    /// </summary>
    public int Quantity { get; set; } = 0;

    /// <summary>
    /// Indicates that the item is canceled
    /// </summary>
    public bool Canceled { get; set; }
}
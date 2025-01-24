namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleRequest
{
    /// <summary>
    /// Gets the id of the sale
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets the list of the items on the sale 
    /// </summary>
    public List<UpdateSaleItemRequest> Items { get; set; } = new();

    /// <summary>
    /// Flag that represents that the sale should be canceled
    /// </summary>
    public bool Cancel { get; set; }
}

public class UpdateSaleItemRequest
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
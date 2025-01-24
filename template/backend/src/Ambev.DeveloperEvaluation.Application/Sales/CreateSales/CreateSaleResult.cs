namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSales;

public class CreateSaleResult
{
    /// <summary>
    /// Gets the number of the sale (it's not the base ID inherited from BaseEntity)
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Gets the date and time when the sale was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
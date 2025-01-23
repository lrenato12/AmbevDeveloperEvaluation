using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

/// <summary>
/// Represent a model that returns informations about the sale, like branch, products and totals
/// that are not retrieved by the single get on the repository
/// </summary>
public class SaleData
{
    /// <summary>
    /// Gets the number of the sale (it's not the base ID inherited from BaseEntity)
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Gets the name of the branch where the sale are created
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets customer name from the customer who created the sale.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets the list of the items on the sales 
    /// </summary>
    public List<SaleDataItem> Items { get; set; } = new();

    /// <summary>
    /// Gets status of the sale
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets the date and time when the sale was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time when the sale was updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// The total value of the sale (sum of its products).
    /// </summary>
    public decimal TotalAmount { get; set; } = 0;

    public SaleData(Sale? sale, Branch? branch, Customer? customer)
    {
        if (sale != null)
        {
            TotalAmount = sale.GetTotalSaleAmount();
            CreatedAt = sale.CreatedAt;
            UpdatedAt = sale.UpdatedAt;
            Status = sale.Status.ToString();
            Number = sale.Number;
        }

        if (branch != null)
            BranchName = branch.Name;

        if (customer != null)
            CustomerName = customer.Name;
    }
}

public class SaleDataItem
{
    /// <summary>
    /// Product of the sale item
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// Quantity of the product ordered
    /// </summary>
    public int Quantity { get; set; } = 0;

    /// <summary>
    /// Original item price applied when the sale was created
    /// </summary>
    public decimal Price { get; set; } = 0;

    /// <summary>
    /// Discount calculated
    /// </summary>
    public decimal Discount { get; set; } = 0;

    /// <summary>
    /// Status of the item
    /// </summary>
    public string StatusMessage { get; set; }

    public SaleDataItem(SaleItem item, Product? product, string message)
    {
        StatusMessage = message;
        ProductName = product is null ? string.Empty : product.Name;
        Quantity = item.Quantity;
        Price = item.Price;
        Discount = item.Discount.GetValueOrDefault();
    }
}
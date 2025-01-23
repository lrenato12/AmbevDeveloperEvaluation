using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a Product on the Sales system
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// The name of the Product
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    // The price of the Product.This price is not fixed and because of this, this value is 
    // registered on the Sale Item value when the Sale is created
    /// </summary>
    public decimal UnitPrice { get; set; } = 0;
}
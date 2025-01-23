using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a customer on the Sales system
/// </summary>
public class Customer : BaseEntity
{
    /// <summary>
    /// Name of the Customer
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Document of the Customer
    /// </summary>
    public string Document { get; set; } = string.Empty;
}
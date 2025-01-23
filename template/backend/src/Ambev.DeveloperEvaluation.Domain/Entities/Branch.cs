using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a branch on the Sales system, where the Sale is created 
/// and where will be prepared to be delivered
/// </summary>
public class Branch : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}
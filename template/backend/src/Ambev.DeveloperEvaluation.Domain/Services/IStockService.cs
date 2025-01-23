using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services;

public interface IStockService
{
    Task<bool> CheckProductAvailabilty(Guid productId, int requiredQuantity, Guid branchId, CancellationToken cancellationToken = default);
    Task<List<SaleItem>> UpdateStockQuantities(Guid branchId, List<SaleItem> items, CancellationToken cancellationToken = default);
}
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IStockRepository
{
    Task<Stock?> GetByBranchAndProductAsync(Guid branchId, Guid productId, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(List<Stock> stock, CancellationToken cancellationToken = default);
}
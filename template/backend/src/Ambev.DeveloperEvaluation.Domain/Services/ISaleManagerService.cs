using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services;

public interface ISaleManagerService
{
    Task<Sale> CreateSale(Sale sale, CancellationToken cancellationToken = default);
    Task<Sale> UpdateSale(Sale sale, CancellationToken cancellationToken = default);
}
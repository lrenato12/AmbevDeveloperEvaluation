using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Services;

public interface ISaleService
{
    Task<SaleData> GetSale(Guid saleId, CancellationToken cancellationToken = default);
}
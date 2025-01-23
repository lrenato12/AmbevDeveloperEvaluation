using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);
    Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Sale?> GetWithItemsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int?> GetLastSaleNumber(CancellationToken cancellationToken = default);
    Task<IEnumerable<Sale>?> GetByCustomerAndDate(Guid customerId, DateTime referenceDate, CancellationToken cancellationToken = default);
}
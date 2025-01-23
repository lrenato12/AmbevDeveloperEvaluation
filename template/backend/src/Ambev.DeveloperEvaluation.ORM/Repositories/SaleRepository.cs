using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of SaleRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new sale in the database
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return sale;
    }

    /// <summary>
    /// Updates a sale in the database
    /// </summary>
    /// <param name="sale">The sale to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale</returns>
    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _context.Entry(sale).State = EntityState.Modified;
        foreach (var item in sale.Items)
            _context.Entry(item).State = EntityState.Modified;

        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);

        return sale;
    }

    /// <summary>
    /// Retrieves a sale by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves the sales created by a customer on a specific date
    /// </summary>
    /// <param name="customerId">The unique identifier of the customer</param>
    /// <param name="referenceDate">The reference date to filter the list of sales</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of sales from a customer</returns>
    public async Task<IEnumerable<Sale>?> GetByCustomerAndDate(Guid customerId, DateTime referenceDate, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Where(o => o.CustomerId == customerId && o.CreatedAt.Date == referenceDate.Date)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves the last sale created on the system and returns the number
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale number, null otherwise</returns>
    public async Task<int?> GetLastSaleNumber(CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => x.Number)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves a sale by their unique identifier with thelist of items
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetWithItemsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(x => x.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
}
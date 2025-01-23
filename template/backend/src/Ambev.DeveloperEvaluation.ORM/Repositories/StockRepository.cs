using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IStockRepository using Entity Framework Core
/// </summary>
public class StockRepository : IStockRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of StockRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public StockRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Checks the available products on a branch by branchId and productId 
    /// </summary>
    /// <param name="branchId">Identifier of the branch</param>
    /// <param name="productId">Identifier of the product</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Stock if exists, null otherwise</returns>
    public async Task<Stock?> GetByBranchAndProductAsync(Guid branchId, Guid productId, CancellationToken cancellationToken = default)
    {
        return await _context.Stocks
            .FirstOrDefaultAsync(x => x.BranchId == branchId && x.ProductId == productId);
    }

    /// <summary>
    /// Updates the list of stock items 
    /// </summary>
    /// <param name="stock">List that will be updated</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task UpdateRangeAsync(List<Stock> stock, CancellationToken cancellationToken = default)
    {
        if (stock != null && stock.Any())
        {
            foreach (var item in stock)
                _context.Entry(item).State = EntityState.Modified;

            _context.Stocks.UpdateRange(stock.ToArray());
            await _context.SaveChangesAsync();
        }
    }
}
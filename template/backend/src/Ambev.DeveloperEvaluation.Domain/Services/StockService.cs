using Ambev.DeveloperEvaluation.Common.EventBroker;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Class to retrieve informations about the Stock of product on specific branchs
/// </summary>
public class StockService : IStockService
{
    private readonly IStockRepository _stockRepository;

    public StockService(
        IStockRepository stockRepository,
        IEventBroker eventBroker)
    {
        _stockRepository = stockRepository;
        RegistrySubscribers(eventBroker);
    }

    /// <summary>
    /// Checks the availabilty of a product on a specific branch
    /// </summary>
    /// <param name="productId">Product required</param>
    /// <param name="requiredQuantity">Quantity of the required product</param>
    /// <param name="branchId">Branch where the product is stocked</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> CheckProductAvailabilty(
        Guid productId,
        int requiredQuantity,
        Guid branchId,
        CancellationToken cancellationToken = default)
    {
        var stockRegistry = await _stockRepository.GetByBranchAndProductAsync(branchId, productId, cancellationToken);

        if (stockRegistry == null)
            return false;

        return stockRegistry.CheckAvailabity(requiredQuantity);
    }

    /// <summary>
    /// Checks the availabilty of a product on a specific branch
    /// </summary>
    /// <param name="branchId">Branch where the products are stocked</param>
    /// <param name="items">The list of itens that should be updated on the stock</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The list of itens that should be updated on the sale</returns>
    public async Task<List<SaleItem>> UpdateStockQuantities(
        Guid branchId,
        List<SaleItem> items,
        CancellationToken cancellationToken = default)
    {
        var stock = new List<Stock>();

        foreach (var item in items)
        {
            var stockRegistry = await _stockRepository.GetByBranchAndProductAsync(branchId, item.ProductId, cancellationToken);
            if (stockRegistry != null)
            {
                var availableValue = stockRegistry.RemoveQuantityFromStockAndReturnsAvailableValue(item.Quantity);
                stock.Add(stockRegistry);

                if (availableValue < item.Quantity)
                {
                    item.Quantity = availableValue;
                    item.StatusMessage = "The quantity of the required item has been subtracted because of the availability";
                }
            }
        }

        await _stockRepository.UpdateRangeAsync(stock, cancellationToken);

        return items;
    }

    private void RegistrySubscribers(IEventBroker eventBroker)
    {
        _ = new Subscriber<SaleCreatedEvent>(eventBroker);
        _ = new Subscriber<SaleCanceledEvent>(eventBroker);
        _ = new Subscriber<SaleModifiedEvent>(eventBroker);
    }
}
using Ambev.DeveloperEvaluation.Common.EventBroker;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Handles operations of create and update sales
/// </summary>
public class SaleManagerService : ISaleManagerService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStockService _stockService;
    private readonly Publisher _publisher;

    public SaleManagerService(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        IStockService stockService,
        IEventBroker eventBroker)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _stockService = stockService;
        _publisher = new Publisher(eventBroker);
    }

    /// <summary>
    /// Creates a new sale entry validating the quantity and rules to allow its creation
    /// </summary>
    /// <param name="sale"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Sale> CreateSale(Sale sale, CancellationToken cancellationToken = default)
    {
        var lastSale = await _saleRepository.GetLastSaleNumber(cancellationToken);
        sale.GenerateSaleNumber(lastSale.GetValueOrDefault());

        ValidateSale(sale);

        sale.Approve();
        await CheckItemsRules(sale, cancellationToken);

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
        var updateItens = await _stockService.UpdateStockQuantities(createdSale.BranchId, createdSale.Items, cancellationToken);

        createdSale.Update(updateItens);
        await UpdateSale(createdSale, cancellationToken);

        _publisher.PublishMessage(new SaleCreatedEvent(sale, "Sale Created"));

        return createdSale;
    }

    /// <summary>
    /// Update a sale on the system
    /// </summary>
    /// <param name="sale"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Sale> UpdateSale(Sale sale, CancellationToken cancellationToken = default)
    {
        ValidateSale(sale);

        await CheckItemsRules(sale, cancellationToken);

        if (sale.Canceled)
        {
            _publisher.PublishMessage(new SaleCanceledEvent(sale, "Sale Canceled"));
        }

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        _publisher.PublishMessage(new SaleModifiedEvent(sale, "Sale Modified"));

        return updatedSale;
    }

    /// <summary>
    /// Validate the sale object and throws exception if it's not valid
    /// </summary>
    /// <param name="sale"></param>
    /// <exception cref="ValidationException"></exception>
    private static void ValidateSale(Sale sale)
    {
        var validator = new SaleValidator();
        var validationResult = validator.Validate(sale);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
    }

    /// <summary>
    /// Obtain the products information and apply business rules to define if the product can be sell 
    /// </summary>
    /// <param name="sale"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task CheckItemsRules(Sale sale, CancellationToken cancellationToken)
    {
        foreach (var item in sale.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            item.Price = product is null ? 0 : product.UnitPrice;

            var isAvailable = await _stockService.CheckProductAvailabilty(item.ProductId, item.Quantity, sale.BranchId, cancellationToken);

            if (isAvailable)
            {
                item.VerifyAllowedQuantity();
                item.CalculateDiscount();
            }
            else
            {
                item.Cancel("This item has been canceled because is out of stock in the moment");
                _publisher.PublishMessage(new ItemCanceledEvent(item, "Item Canceled"));
            }
        }
    }
}
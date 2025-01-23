using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Service class to obtain all the information that are denormalized into Sales table
/// Some information could be in external services, but for simplicity it's included on the same database  
/// </summary>
public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;

    public SaleService(
        ISaleRepository saleRepository,
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
        _productRepository = productRepository;
    }

    /// <summary>
    /// Gets the sale information including products, branch and customer
    /// </summary>
    /// <param name="saleId">The unique identifier of the sale</param>
    /// <param name="cancellationToken"></param>
    /// <returns>SaleData value object/model</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<SaleData> GetSale(Guid saleId, CancellationToken cancellationToken = default)
    {
        var sale = await _saleRepository.GetWithItemsByIdAsync(saleId, cancellationToken);

        if (sale is null)
            throw new KeyNotFoundException("Sale not found");

        var branch = await _branchRepository.GetByIdAsync(sale.BranchId, cancellationToken);
        var customer = await _customerRepository.GetByIdAsync(sale.CustomerId, cancellationToken);

        var saleData = new SaleData(sale, branch, customer);
        await GetProducts(saleData, sale.Items, cancellationToken);

        return saleData;
    }

    /// <summary>
    /// Adds product information on the list of itens 
    /// </summary>
    /// <param name="saleData">Sale data object that will include the items</param>
    /// <param name="items">Sale items from the database</param>
    /// <param name="cancellationToken"></param>
    private async Task GetProducts(SaleData saleData, List<SaleItem> items, CancellationToken cancellationToken)
    {
        foreach (var item in items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            saleData.Items.Add(new SaleDataItem(item, product, item.StatusMessage));
        }
    }
}
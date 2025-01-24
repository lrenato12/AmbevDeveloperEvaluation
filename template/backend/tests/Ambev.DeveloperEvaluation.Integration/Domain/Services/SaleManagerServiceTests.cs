using Ambev.DeveloperEvaluation.Common.EventBroker;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Integration.Domain.Services.TestData;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Domain.Services;

/// <summary>
/// Sale Manager Service Tests
/// </summary>
public class SaleManagerServiceTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStockService _stockService;
    private readonly SaleManagerService _saleService;

    public SaleManagerServiceTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _stockService = Substitute.For<IStockService>();
        _saleService = new SaleManagerService(_saleRepository, _productRepository, _stockService, Substitute.For<IEventBroker>());
    }

    /// <summary>
    /// Test - Given valid sale then should return the sale created
    /// </summary>
    [Fact(DisplayName = "Given valid sale then should return the sale created")]
    public async Task Given_ValidSaleRequest_Then_ShouldReturnTheCreatedSale()
    {
        var sale = SaleManagerServiceTestData.GenerateSale();
        var items = SaleManagerServiceTestData.GenerateValidListOfItems();
        sale.Items = items;

        _saleRepository.GetLastSaleNumber().Returns(1);
        _saleRepository.CreateAsync(Arg.Any<Sale>()).Returns(sale);
        _productRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(SaleManagerServiceTestData.GenerateProduct());
        _stockService.CheckProductAvailabilty(Arg.Any<Guid>(), Arg.Any<int>(), Arg.Any<Guid>()).Returns(true);

        var result = await _saleService.CreateSale(sale, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(SaleStatus.Approved, result.Status);
        Assert.NotEqual(0, result.GetTotalSaleAmount());
    }

    /// <summary>
    /// Test - Given invalid sale then should return errors
    /// </summary>
    [Fact(DisplayName = "Given invalid sale then should return errors")]
    public async Task Given_InvalidSale_Then_ShouldThrowErrors()
    {
        var sale = SaleManagerServiceTestData.GenerateInvalidSale();
        _saleRepository.GetLastSaleNumber().Returns(1);

        var act = () => _saleService.CreateSale(sale, CancellationToken.None);

        await Assert.ThrowsAsync<ValidationException>(act);
    }

    /// <summary>
    /// Test - Given valid updated sale then should return the sale created
    /// </summary>
    [Fact(DisplayName = "Given valid updated sale then should return the sale created")]
    public async Task Given_ValidUpdatedSale_Then_ShouldReturnSale()
    {
        var sale = SaleManagerServiceTestData.GenerateSale();
        var items = SaleManagerServiceTestData.GenerateValidListOfItems();

        sale.Items = items;
        sale.Update(items, false);

        _saleRepository.UpdateAsync(Arg.Any<Sale>()).Returns(sale);
        _productRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(SaleManagerServiceTestData.GenerateProduct());
        _stockService.CheckProductAvailabilty(Arg.Any<Guid>(), Arg.Any<int>(), Arg.Any<Guid>()).Returns(true);

        var result = await _saleService.UpdateSale(sale, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(SaleStatus.Approved, result.Status);
        Assert.NotEqual(0, result.GetTotalSaleAmount());
    }

    /// <summary>
    /// Test - Given valid sale when stock is unavailable then should return the sale created
    /// </summary>
    [Fact(DisplayName = "Given valid sale when stock is unavailable then should return the sale created")]
    public async Task Given_ValidSaleRequest_WhenStockIsUnavailable_Then_ShouldCancelTheItems()
    {
        var sale = SaleManagerServiceTestData.GenerateSale();
        var items = SaleManagerServiceTestData.GenerateValidListOfItems();
        sale.Items = items;

        _saleRepository.GetLastSaleNumber().Returns(1);
        _saleRepository.CreateAsync(Arg.Any<Sale>()).Returns(sale);
        _productRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(SaleManagerServiceTestData.GenerateProduct());
        _stockService.CheckProductAvailabilty(Arg.Any<Guid>(), Arg.Any<int>(), Arg.Any<Guid>()).Returns(false);

        var result = await _saleService.CreateSale(sale, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(0, result.GetTotalSaleAmount());
    }
}
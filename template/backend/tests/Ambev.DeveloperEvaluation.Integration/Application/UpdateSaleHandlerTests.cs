using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Integration.Application.TestData;
using Ambev.DeveloperEvaluation.Integration.Domain.Services.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Application;

/// <summary>
/// Update Sale Handler Tests
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleManagerService _saleService;
    private readonly IMapper _mapper;
    private readonly UpdateSaleHandler _handler;

    /// <summary>
    /// Update Sale Handler Tests
    /// </summary>
    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _saleService = Substitute.For<ISaleManagerService>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdateSaleHandler(_saleRepository, _saleService, _mapper);
    }

    /// <summary>
    /// Test - Given valid sale data When updating sale Then returns success response
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When updating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        var command = CreateSaleHandlerTestData.CreateValidUpdateCommand();
        var sale = SaleManagerServiceTestData.GenerateSale();
        _saleService.UpdateSale(Arg.Any<Sale>()).Returns(sale);
        _saleRepository.GetWithItemsByIdAsync(Arg.Any<Guid>()).Returns(sale);

        var result = new UpdateSaleResult
        {
            CreatedAt = DateTime.UtcNow,
            Number = 1
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<UpdateSaleResult>(sale).Returns(result);

        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        createSaleResult.Should().NotBeNull();
        createSaleResult.Number.Should().Be(1);
        await _saleService.Received(1).UpdateSale(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Test - Given an invalid sale data When updating sale Then returns exception
    /// </summary>
    [Fact(DisplayName = "Given an invalid sale data When updating sale Then returns exception")]
    public async Task Handle_InvalidRequest_ReturnsException()
    {
        var command = CreateSaleHandlerTestData.CreateInvalidUpdateCommand();

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}
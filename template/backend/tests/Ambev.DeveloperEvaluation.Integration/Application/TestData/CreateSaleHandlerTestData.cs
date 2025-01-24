using Ambev.DeveloperEvaluation.Application.Sales.CreateSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

namespace Ambev.DeveloperEvaluation.Integration.Application.TestData;

public static class CreateSaleHandlerTestData
{
    /// <summary>
    /// Create Valid Command
    /// </summary>
    /// <returns></returns>
    public static CreateSaleCommand CreateValidCommand()
    {
        return new CreateSaleCommand()
        {
            BranchId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            Items = new List<CreateSaleItemCommand>()
            {
                new CreateSaleItemCommand()
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 10
                }
            }
        };
    }

    /// <summary>
    /// Create Valid Update Command
    /// </summary>
    /// <returns></returns>
    public static UpdateSaleCommand CreateValidUpdateCommand()
    {
        return new UpdateSaleCommand()
        {
            SaleId = Guid.NewGuid(),
            Items = new List<UpdateSaleItemCommand>()
            {
                new UpdateSaleItemCommand()
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 10,
                    Canceled = true
                }
            }
        };
    }

    /// <summary>
    /// Create Invalid Update Command
    /// </summary>
    /// <returns></returns>
    public static UpdateSaleCommand CreateInvalidUpdateCommand()
    {
        return new UpdateSaleCommand()
        {
            SaleId = Guid.Empty,
            Items = new List<UpdateSaleItemCommand>()
        };
    }

    /// <summary>
    /// Create Invalid Command
    /// </summary>
    /// <returns></returns>
    public static CreateSaleCommand CreateInvalidCommand()
    {
        return new CreateSaleCommand()
        {
            BranchId = Guid.Empty,
            CustomerId = Guid.Empty,
            Items = new List<CreateSaleItemCommand>()
        };
    }
}
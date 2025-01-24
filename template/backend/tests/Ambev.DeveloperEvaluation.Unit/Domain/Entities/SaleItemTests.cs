﻿using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Sale Item Tests
/// </summary>
public class SaleItemTests
{
    /// <summary>
    /// Test - Item discount should be calculated
    /// </summary>
    [Theory(DisplayName = "Item discount should be calculated")]
    [InlineData(0, false)]
    [InlineData(4, true)]
    [InlineData(10, true)]
    [InlineData(20, true)]
    [InlineData(30, false)]
    public void Given_WhenCalculateDiscount_Then_ValueShouldReturned(int quantity, bool hasDiscount)
    {
        var item = SaleTestData.GenerateSaleItem();
        item.Quantity = quantity;

        item.CalculateDiscount();

        Assert.Equal(hasDiscount, item.HasDiscount);
        Assert.NotNull(item.StatusMessage);
    }

    /// <summary>
    /// Test - Item quantity should be verified
    /// </summary>
    [Fact(DisplayName = "Item quantity should be verified")]
    public void Given_WhenVerifyQuantityAllowed_Then_CanceledStatusShouldBeSet()
    {
        var item = SaleTestData.GenerateSaleItem();
        item.Quantity = 30;

        item.VerifyAllowedQuantity();

        Assert.True(item.Canceled);
        Assert.Equal(0, item.Price);
        Assert.NotNull(item.StatusMessage);
    }

    /// <summary>
    /// Test - The Total Amount should be returned
    /// </summary>
    [Fact(DisplayName = "The Total Amount should be returned")]
    public void Given_WhenCalculateTheTotalAmount_Then_TheValueShouldBeReturned()
    {
        var item = SaleTestData.GenerateSaleItem();
        item.Quantity = 4;
        item.Price = 5.90m;
        item.Discount = 0.10m;

        item.CalculateDiscount();
        var result = item.TotalAmount;

        Assert.Equal(21.24m, result);
    }

    /// <summary>
    /// Test - The flag for discount should be true if it has been applied
    /// </summary>
    [Fact(DisplayName = "The flag for discount should be true if it has been applied")]
    public void Given_WhenVerifyIfDiscountHasBeenApplied_Then_ReturnsTrue()
    {
        var item = SaleTestData.GenerateSaleItem();
        item.Quantity = 4;
        item.Price = 5.90m;
        item.Discount = 0.10m;

        item.CalculateDiscount();

        Assert.True(item.HasDiscount);
    }

    /// <summary>
    /// Test - The flag for discount should be true if it has been applied
    /// </summary>
    [Fact(DisplayName = "The flag for discount should be true if it has been applied")]
    public void Given_WhenUpdateQuantity_Then_RulesShouldBeApplied()
    {
        var item = SaleTestData.GenerateSaleItem();
        item.UpdateQuantity(5, false);

        Assert.True(item.HasDiscount);
        Assert.False(item.Canceled);
    }
}
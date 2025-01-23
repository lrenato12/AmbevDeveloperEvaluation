namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represent a item of the sale, with a product, the price calculated with discount and quantity 
/// </summary>
public class SaleItem
{
    /// <summary>
    /// The rereferenced id of the sale
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Id of the product
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Quantity of the product ordered
    /// </summary>
    public int Quantity { get; set; } = 0;

    /// <summary>
    /// The calculated price on the moment when the Sale was created. 
    /// If it has a discount, the price will be different of the product price 
    /// available on the moment of the sale
    /// </summary>
    public decimal Price { get; set; } = 0;

    /// <summary>
    /// The discount applied in the item defined by the following business rules: 
    /// - Purchases above 4 identical items have a 10% discount
    /// - Purchases between 10 and 20 identical items have a 20% discount
    /// - It's not possible to sell above 20 identical items
    /// - Purchases below 4 items cannot have a discount
    /// </summary>
    public decimal? Discount { get; set; } = 0;

    /// <summary>
    /// Flag that indicates if the item has been canceled
    /// </summary>
    public bool Canceled { get; set; } = false;

    /// <summary>
    /// Text that will be returned if the item has discount or have been canceled
    /// </summary>
    public string StatusMessage { get; set; } = string.Empty;

    /// <summary>
    /// Returns the TotalAMount of the SaleItem
    /// </summary>
    public decimal TotalAmount => !Canceled ? (Price - Discount.GetValueOrDefault()) * Quantity : 0;

    /// <summary>
    /// Indicate if the item has discount
    /// </summary>
    public bool HasDiscount => Discount > 0;

    /// <summary>
    /// Calculate the discount of the product based on the business rules
    /// </summary>
    public void CalculateDiscount()
    {
        if (!Canceled)
        {
            if (Quantity < 4)
                SetDiscountValue(0);

            if (Quantity >= 4 && Quantity < 10)
                SetDiscountValue(10);

            if (Quantity >= 10 && Quantity <= 20)
                SetDiscountValue(20);
        }
    }

    /// <summary>
    /// Verify the quantity allowed of the same product and cancel the item if it exceeds the limit
    /// </summary>
    public void VerifyAllowedQuantity()
    {
        if (Quantity > 20)
        {
            Canceled = true;
            Price = 0;
            Discount = 0;
            StatusMessage = "This item has been canceled because it exceeds the limit allowed per product";
        }
    }

    /// <summary>
    /// Update the quantity and verify the rules for items of the sale, as validate quantities, discounts, etc 
    /// </summary>
    public void UpdateQuantity(int quantity, bool canceled)
    {
        Quantity = quantity;
        Canceled = canceled;
        CalculateDiscount();
        VerifyAllowedQuantity();
    }

    /// <summary>
    /// Cancel an item for not following business rules
    /// </summary>
    public void Cancel(string message)
    {
        Canceled = true;
        StatusMessage = message;
    }

    /// <summary>
    /// Apply the percentage value based on the product price on the Discount property 
    /// </summary>
    /// <param name="discountPercentage">The percentage to be applied to the price</param>
    private void SetDiscountValue(decimal discountPercentage)
    {
        Discount = (discountPercentage > 0 && Price > 0) ? (Price * discountPercentage) / 100 : 0;
        StatusMessage = Discount > 0 ? $"Discount of {discountPercentage}% applied" : "No discount applied";
    }
}
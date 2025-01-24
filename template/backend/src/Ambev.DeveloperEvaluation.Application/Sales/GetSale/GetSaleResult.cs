using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleResult
    {
        /// <summary>
        /// Gets the number of the sale (it's not the base ID inherited from BaseEntity)
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets the name of the branch where the sale are created
        /// </summary>
        public string BranchName { get; set; } = string.Empty;

        /// <summary>
        /// Gets customer name from the customer who created the sale.
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// Gets the list of the items on the sales 
        /// </summary>
        public List<SaleDataItem> Items { get; set; } = new();

        /// <summary>
        /// Gets status of the sale
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets the date and time when the sale was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets the date and time when the sale was updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// The total value of the sale (sum of its products).
        /// </summary>
        public decimal TotalAmount { get; set; } = 0;
    }
}
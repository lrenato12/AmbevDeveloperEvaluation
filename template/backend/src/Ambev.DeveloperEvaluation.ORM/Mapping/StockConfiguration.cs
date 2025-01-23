using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.ToTable("Stock");
        builder.HasKey(si => new { si.BranchId, si.ProductId });

        builder.Property(u => u.BranchId).IsRequired();
        builder.Property(u => u.ProductId).IsRequired();
        builder.Property(u => u.AvailableQuantity).IsRequired();
        builder.Property(u => u.UpdatedAt).IsRequired();
    }
}
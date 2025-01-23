using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.Property(u => u.SaleId).IsRequired();
        builder.Property(u => u.ProductId).IsRequired();
        builder.Property(u => u.Quantity).IsRequired();
        builder.Property(u => u.Price).IsRequired();
        builder.Property(u => u.Discount).IsRequired(false);
        builder.Property(u => u.StatusMessage).IsRequired(false);
        builder.Property(u => u.Canceled).IsRequired().HasDefaultValue(false);

        builder.HasKey(si => new { si.SaleId, si.ProductId });
    }
}
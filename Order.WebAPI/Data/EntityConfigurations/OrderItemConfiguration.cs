using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.WebAPI.Models.Entities;

namespace Order.WebAPI.Data.EntityConfigurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.ProductId)
            .IsRequired();

        builder.Property(oi => oi.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(oi => oi.ProductSku)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(oi => oi.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        // Ignore computed property for EF
        builder.Ignore(oi => oi.TotalPrice);
    }
}
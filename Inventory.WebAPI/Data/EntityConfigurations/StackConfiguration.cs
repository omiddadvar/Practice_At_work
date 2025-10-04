using Inventory.WebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.WebAPI.Data.EntityConfigurations;

public class StackConfiguration : IEntityTypeConfiguration<Stack>
{
    public void Configure(EntityTypeBuilder<Stack> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Quantity)
            .IsRequired();

        builder.Property(s => s.LocationInWarehouse)
            .HasMaxLength(100);

        // Relationships
        builder.HasOne(s => s.Product)
            .WithMany(p => p.Stacks)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.WareHouse)
            .WithMany(w => w.Stacks)
            .HasForeignKey(s => s.WareHouseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Composite index to ensure unique product in warehouse
        builder.HasIndex(s => new { s.ProductId, s.WareHouseId })
            .IsUnique();
    }
}
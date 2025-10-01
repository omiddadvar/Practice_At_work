using Inventory.WebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Inventory.WebAPI.Data.EntityConfigurations;

public class WareHouseConfiguration : IEntityTypeConfiguration<WareHouse>
{
    public void Configure(EntityTypeBuilder<WareHouse> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(w => w.Location)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(w => w.Capacity)
            .IsRequired();

        builder.HasIndex(w => w.Name)
            .IsUnique();
    }
}

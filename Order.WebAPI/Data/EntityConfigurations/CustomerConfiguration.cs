using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.WebAPI.Models.Entities;

namespace Order.WebAPI.Data.EntityConfigurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.Address)
            .HasMaxLength(500);

        builder.Property(c => c.City)
            .HasMaxLength(100);

        builder.Property(c => c.State)
            .HasMaxLength(50);

        builder.Property(c => c.ZipCode)
            .HasMaxLength(20);

        builder.HasIndex(c => c.Email)
            .IsUnique();
    }
}
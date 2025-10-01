using Inventory.WebAPI.Data.EntityConfigurations;
using Inventory.WebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.WebAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<WareHouse> WareHouses { get; set; }
    public DbSet<Stack> Stacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new WareHouseConfiguration());
        modelBuilder.ApplyConfiguration(new StackConfiguration());
    }
}
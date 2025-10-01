namespace Inventory.WebAPI.Models.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }

    // Navigation properties
    public virtual Category Category { get; set; } = null!;
    public virtual ICollection<Stack> Stacks { get; set; } = new List<Stack>();
}
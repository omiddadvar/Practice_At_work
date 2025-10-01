namespace Inventory.WebAPI.Models.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
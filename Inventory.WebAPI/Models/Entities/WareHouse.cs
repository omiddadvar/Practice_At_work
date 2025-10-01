namespace Inventory.WebAPI.Models.Entities;

public class WareHouse : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int Capacity { get; set; }

    // Navigation properties
    public virtual ICollection<Stack> Stacks { get; set; } = new List<Stack>();
}
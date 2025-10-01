namespace Inventory.WebAPI.Models.Entities;

public class Stack : BaseEntity
{
    public int ProductId { get; set; }
    public int WareHouseId { get; set; }
    public int Quantity { get; set; }
    public string LocationInWarehouse { get; set; } = string.Empty;

    // Navigation properties
    public virtual Product Product { get; set; } = null!;
    public virtual WareHouse WareHouse { get; set; } = null!;
}
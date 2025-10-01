namespace Inventory.WebAPI.Models.DTOs;

public class StackDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int WareHouseId { get; set; }
    public int Quantity { get; set; }
    public string LocationInWarehouse { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string WareHouseName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateStackDto
{
    public int ProductId { get; set; }
    public int WareHouseId { get; set; }
    public int Quantity { get; set; }
    public string LocationInWarehouse { get; set; } = string.Empty;
}

public class UpdateStackDto
{
    public int Quantity { get; set; }
    public string LocationInWarehouse { get; set; } = string.Empty;
}
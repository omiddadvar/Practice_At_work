namespace Inventory.WebAPI.Models.DTOs;

public class WareHouseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int CurrentStock { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateWareHouseDto
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int Capacity { get; set; }
}

public class UpdateWareHouseDto
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int Capacity { get; set; }
}
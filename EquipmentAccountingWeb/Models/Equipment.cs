namespace EquipmentAccountingWeb.Models;

public enum EquipmentStatus { Active, InRepair, WrittenOff }

public class Equipment 
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string InventoryNumber { get; set; }
    public EquipmentStatus Status { get; set; } = EquipmentStatus.Active;
    public string ClassroomNumber { get; set; }
}

public class Computer : Equipment { }
public class Projector : Equipment { }
using EquipmentAccountingWeb.Models;

namespace EquipmentAccountingWeb.Data;

public class InventoryContext 
{
    private static InventoryContext _instance;
    private InventoryContext() { }
    public static InventoryContext Instance => _instance ??= new InventoryContext();

    public List<Equipment> Equipments { get; } = new List<Equipment>();
    public List<string> HistoryLogs { get; } = new List<string>();

    public List<User> Users { get; } = new List<User>
    {
        new User { Login = "admin", Password = "123", FullName = "Андрій (Адмін)", Role = UserRole.Admin },
        new User { Login = "mvo", Password = "123", FullName = "Сергій (МВО)", Role = UserRole.ResponsiblePerson },
        new User { Login = "user", Password = "123", FullName = "Викладач", Role = UserRole.Viewer }
    };
}
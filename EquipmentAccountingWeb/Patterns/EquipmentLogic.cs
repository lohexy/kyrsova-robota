using EquipmentAccountingWeb.Models;

namespace EquipmentAccountingWeb.Patterns;

public static class EquipmentFactory {
    public static Equipment Create(string type, string name, string invNum) {
        return type.ToLower() switch {
            "pc" => new Computer { Name = name, InventoryNumber = invNum },
            "projector" => new Projector { Name = name, InventoryNumber = invNum },
            _ => throw new ArgumentException("Unknown type")
        };
    }
}

public interface IReportStrategy {
    string Generate(List<Equipment> data);
}

public class CsvReportStrategy : IReportStrategy {
    public string Generate(List<Equipment> data) {
        var res = "InventoryNumber;Name;Room;Status\n";
        data.ForEach(e => res += $"{e.InventoryNumber};{e.Name};{e.ClassroomNumber};{e.Status}\n");
        return res;
    }
}
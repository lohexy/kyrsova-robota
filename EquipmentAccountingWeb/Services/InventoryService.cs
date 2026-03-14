using EquipmentAccountingWeb.Data;
using EquipmentAccountingWeb.Models;

namespace EquipmentAccountingWeb.Services;

public class InventoryService {
    private readonly InventoryContext _db = InventoryContext.Instance;

    public void UpdateEquipment(Equipment updated) {
        var existing = _db.Equipments.FirstOrDefault(e => e.Id == updated.Id);
        if (existing != null) {
            existing.Name = updated.Name;
            existing.ClassroomNumber = updated.ClassroomNumber;
            existing.Status = updated.Status;
            _db.HistoryLogs.Add($"{DateTime.Now}: Оновлено {existing.InventoryNumber}");
        }
    }
    public void AddEquipment(Equipment newEquipment) {
    if (_db.Equipments.Any(e => e.InventoryNumber == newEquipment.InventoryNumber)) {
        throw new Exception("Обладнання з таким інвентарним номером вже існує в базі!");
    }

    newEquipment.Id = Guid.NewGuid();

    _db.Equipments.Add(newEquipment);
    _db.HistoryLogs.Add($"{DateTime.Now}: Додано нове обладнання {newEquipment.InventoryNumber}");
}
}
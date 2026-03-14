using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EquipmentAccountingWeb.Data;
using EquipmentAccountingWeb.Models;
using EquipmentAccountingWeb.Services;
using EquipmentAccountingWeb.Patterns;

namespace EquipmentAccountingWeb.Controllers;

[Authorize]
public class EquipmentController : Controller
{
    private readonly InventoryService _inventoryService = new InventoryService();
    private readonly InventoryContext _db = InventoryContext.Instance;
    private readonly InventoryService _service;

    public EquipmentController(InventoryService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        return View(_db.Equipments);
    }

    public IActionResult Export()
    {
        var strategy = new CsvReportStrategy();
        var content = strategy.Generate(_db.Equipments);
        return File(System.Text.Encoding.UTF8.GetBytes(content), "text/csv", "report.csv");
    }

    [HttpGet]
public IActionResult Edit(Guid id)
{
    var item = _db.Equipments.FirstOrDefault(e => e.Id == id);
    if (item == null) return NotFound();
    return View(item);
}

[HttpPost]
public IActionResult Edit(Computer model)
{
    _service.UpdateEquipment(model);
    return RedirectToAction("Index");
}

[HttpPost]
public IActionResult AddEquipment(Equipment newEquipment)
{
    if (ModelState.IsValid)
    {
        try
        {
            _inventoryService.AddEquipment(newEquipment);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
    }
    
    return View(newEquipment); 
}

[HttpGet]
public IActionResult AddEquipment()
{
    return View(); 
}

}

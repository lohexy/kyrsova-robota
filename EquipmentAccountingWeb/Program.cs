using EquipmentAccountingWeb.Data;
using EquipmentAccountingWeb.Services;
using EquipmentAccountingWeb.Patterns;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// --- 1. РЕЄСТРАЦІЯ СЕРВІСІВ ---
builder.Services.AddControllersWithViews();

// Реєструємо твої класи
builder.Services.AddSingleton(InventoryContext.Instance);
builder.Services.AddScoped<InventoryService>();

// Налаштування безпеки (Куки)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => 
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

// --- 2. ПОЧАТКОВІ ДАНІ (SEED DATA) ---
var db = InventoryContext.Instance;
if (!db.Equipments.Any()) {
    var item = EquipmentFactory.Create("pc", "Dell Optiplex", "INV-001");
    item.ClassroomNumber = "101";
    db.Equipments.Add(item);
}

// --- 3. НАЛАШТУВАННЯ ОБРОБКИ ЗАПИТІВ (MIDDLEWARE) ---
// ПОРЯДОК ТУТ ДУЖЕ ВАЖЛИВИЙ!
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Хто ти? (Це має бути першим)
app.UseAuthorization();  // Що тобі можна? (Це другим)

// --- 4. МАРШРУТИ (ROUTES) ---
app.MapControllerRoute(
    name: "list",
    pattern: "list",
    defaults: new { controller = "Equipment", action = "Index" });

app.MapControllerRoute(
    name: "home",
    pattern: "home",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EquipmentAccountingWeb.Models;
using EquipmentAccountingWeb.Data; 

namespace EquipmentAccountingWeb.Controllers;

public class AccountController : Controller 
{
    private readonly InventoryContext _db = InventoryContext.Instance;

    [HttpGet] 
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string login, string password) 
    {
        var user = _db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
        
        if (user == null) 
        {
            ModelState.AddModelError("", "Невірний логін або пароль!");
            return View();
        }

        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("Login", user.Login)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout() 
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


    [Authorize(Roles = "Admin")]
    public IActionResult Users() 
    {
        return View(_db.Users);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult EditUser(string login) 
    {
        var user = _db.Users.FirstOrDefault(u => u.Login == login);
        if (user == null) return NotFound();
        return View(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult EditUser(User updatedUser) 
    {
        var user = _db.Users.FirstOrDefault(u => u.Login == updatedUser.Login);
        if (user != null) 
        {
            user.FullName = updatedUser.FullName;
            user.Role = updatedUser.Role;
            if (!string.IsNullOrEmpty(updatedUser.Password)) {
                user.Password = updatedUser.Password;
            }
        }
        return RedirectToAction("Users");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult CreateUser() 
    {
        return View(new User());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult CreateUser(User newUser) 
    {
        if (_db.Users.Any(u => u.Login == newUser.Login))
        {
            ModelState.AddModelError("Login", "Користувач із таким логіном вже існує!");
            return View(newUser);
        }

        // Додаємо в базу
        _db.Users.Add(newUser);
        return RedirectToAction("Users");
    }
}
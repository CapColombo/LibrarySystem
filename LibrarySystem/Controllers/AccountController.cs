using LibrarySystem.Models;
using LibrarySystem.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibrarySystem.Controllers;

public class AccountController : Controller
{
    private readonly IUserRepository _repository;

    public AccountController(IUserRepository repository)
    {
        _repository = repository;
    }

    public ViewResult Login() => View();

    public ViewResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            User user = await _repository.Users.FirstOrDefaultAsync(u => u.Login == model.Login
                && u.Password == model.Password);
            if (user != null)
            {
                await Authenticate(model.Login);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            User user = await _repository.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
            if (user == null)
            {
                _repository.Add(new User { Login = model.Login, Password = model.Password });
                await Authenticate(model.Login);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", $"Логин {user.Login} уже занят!");
        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }

    private async Task Authenticate(string login)
    {
        var claims = new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, login) };
        ClaimsIdentity id = new(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }
}

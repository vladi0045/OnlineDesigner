using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineDesigner.Data;
using OnlineDesigner.Models;
using OnlineDesigner.ViewModels;

namespace OnlineDesigner.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly OnlineDesignerContext _context;
    private readonly IdentityContext _identityContext;
    private readonly SignInManager<User> _signInManager;

    public enum Filter
    {
        Username,
        Role
    }

    public AccountController(OnlineDesignerContext context, IdentityContext identityContext, SignInManager<User> signInManager)
    {
        _context = context;
        _identityContext = identityContext;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([Bind("Username,Password,RememberMe")] Login login)
    {
        var user = await _identityContext.LogInUserAsync(login.Username, login.Password);

        if (user != null)
        {
            await _signInManager.SignInAsync(user, login.RememberMe);
            return RedirectToAction("Index", "Home");
        }

        return View(login);

    }

    public async Task<IActionResult> Registration()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registration([Bind("Username,Password,ConfirmPassword")] Register register)
    {
        if (ModelState.IsValid)
        {
            var username = register.Username;
            var password = register.Password;

            if (await _identityContext.FindUserByNameAsync(username) != null)
            {
                return View(register);
            }

            await _identityContext.CreateUserAsync(username, password, Role.Customer);
            return RedirectToAction(nameof(Login));
        }
        else
            return View(register);
    }

    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> AccessDenied()
    {
        return View();
    }

}
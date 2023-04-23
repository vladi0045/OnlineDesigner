using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineDesigner.Data;
using OnlineDesigner.Models;
using OnlineDesigner.ViewModels;

namespace OnlineDesigner.Controllers;

//[Authorize(Roles = "Administrator")]
public class UsersController : Controller
{
    private readonly OnlineDesignerContext _context;
    private readonly IdentityContext _identityContext;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public enum Filter
    {
        Username,
        Role
    }

    public UsersController(OnlineDesignerContext context, IdentityContext identityContext, SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _context = context;
        _identityContext = identityContext;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string searchString, int filter)
    {
        var users = await _identityContext.GetAllUsersAsync();

        if (!string.IsNullOrEmpty(searchString))
        {
            users = filter switch
            {
                0 => users.Where(p => p.UserName.Contains(searchString)),
                3 => users.Where(p => p.Role.ToString().Contains(searchString)),
                _ => users
            };
        }

        return View(users);
    }
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Username,Password,ConfirmPassword,Role")] Register register)
    {
        if (ModelState.IsValid)
        {
            var username = register.Username;
            var password = register.Password;
            var role = register.Role;

            if (await _identityContext.FindUserByNameAsync(username) != null)
            {
                return View(register);
            }

            await _identityContext.CreateUserAsync(username, password, role);
            return RedirectToAction(nameof(Index));
        }
        else
            return View(register);
    }

    public async Task<IActionResult> Info(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Info(string id, [Bind("Username,Role")] User user)
    {
        if (ModelState.IsValid)
        {
            user = await _context.Users.FindAsync(id);

            var username = user.UserName;
            var role = user.Role;

            await _identityContext.UpdateUserAsync(username, role);
            return RedirectToAction(nameof(Index));
        }

        return View(user);
    }

    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (_context.Users == null)
        {
            return Problem("Entity set is null.");
        }

        var user = await _context.Users.FindAsync(id);

        if (user != null)
        {
            await _identityContext.DeleteUserByNameAsync(user.UserName);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ChangePassword(string id)
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(string id, [Bind("CurrentPassword,NewPassword,ConfirmPassword")] Password password)
    {
        var user = await _context.Users.FindAsync(id);

        if (user != null)
        {
            await _identityContext.ChangeUserPasswordAsync(user, password.CurrentPassword,
                password.NewPassword);
        }

        return RedirectToAction(nameof(Index));
    }
}
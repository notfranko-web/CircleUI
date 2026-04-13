using CircleUI.Core.Services;
using CircleUI.Data;
using CircleUI.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CircleUI.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;

    public AdminController(UserManager<User> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var assetService = new AssetService(_context);
        var componentService = new ComponentService(_context);

        var users = _userManager.Users.ToList();
        var assets = await assetService.GetAll();
        var components = await componentService.GetAll();

        var userRoles = new Dictionary<string, bool>();
        foreach (var user in users)
        {
            userRoles[user.Id] = await _userManager.IsInRoleAsync(user, "Admin");
        }

        ViewBag.Users = users;
        ViewBag.UserRoles = userRoles;
        ViewBag.AssetCount = assets.Count;
        ViewBag.ComponentCount = components.Count;
        ViewBag.UserCount = users.Count;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> MakeAdmin(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
            await _userManager.AddToRoleAsync(user, "Admin");

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveAdmin(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null && user.Email != "admin@circleui.com")
            await _userManager.RemoveFromRoleAsync(user, "Admin");

        return RedirectToAction("Index");
    }
}

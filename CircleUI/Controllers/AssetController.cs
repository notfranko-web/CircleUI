using CircleUI.Core.DTOs;
using CircleUI.Core.Interfaces;
using CircleUI.Core.Services;
using CircleUI.Data;
using CircleUI.Data.Models;
using CircleUI.ViewModels;
using CircleUI.ViewModels.Asset;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Diagnostics;

namespace CircleUI.Controllers;

[Authorize]
public class AssetController : Controller
{
    private readonly IAssetService _service;
    private readonly UserManager<User> _userManager;
    private readonly IWebHostEnvironment _environment;

    public AssetController(ApplicationDbContext context, UserManager<User> userManager, IWebHostEnvironment environment)
    {
        _service = new AssetService(context);
        _userManager = userManager;
        _environment = environment;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Json(new { success = false, message = "No file selected" });

        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", userId);
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var asset = new AssetDTO
        {
            FileName = file.FileName,
            MimeType = file.ContentType,
            SizeBytes = file.Length,
            StoragePath = $"/uploads/{userId}/{fileName}",
            UserId = userId
        };

        var created = await _service.Create(asset);
        return Json(new { success = true, asset = created });
    }

    [HttpGet]
    public async Task<IActionResult> GetMyAssets()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var assets = await _service.GetByUserId(userId);
        return Json(assets);
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var assets = await _service.GetAll();
        var userIds = assets.Select(a => a.UserId).Distinct().ToList();
        var userNames = new Dictionary<string, string>();
        foreach (var uid in userIds)
        {
            var user = await _userManager.FindByIdAsync(uid);
            userNames[uid] = user?.DisplayName ?? user?.UserName ?? uid;
        }
        ViewBag.UserNames = userNames;
        return View(assets);
    }
    // CREATE
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var asset = new AssetCreateViewModel();
        return View(asset);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AssetCreateViewModel input)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(input);
        }
        AssetDTO asset = new AssetDTO()
        {
            FileName = input.FileName,
            MimeType = input.MimeType,
            SizeBytes = input.SizeBytes,
            StoragePath = input.StoragePath,
            UserId = _userManager.GetUserId(User)!
        };
        await _service.Create(asset);
        return RedirectToAction("Index");
    }
    // UPDATE
    public async Task<IActionResult> Update(string id)
    {
        AssetDTO existing = await _service.GetById(id);
        AssetUpdateViewModel model = new AssetUpdateViewModel()
        {
            FileName = existing.FileName,
            MimeType = existing.MimeType,
            SizeBytes = existing.SizeBytes,
            StoragePath = existing.StoragePath,
            UserId = existing.UserId
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(AssetUpdateViewModel input)
    {
        AssetDTO asset = new AssetDTO()
        {
            Id = input.Id,
            FileName = input.FileName,
            MimeType = input.MimeType,
            SizeBytes = input.SizeBytes,
            StoragePath = input.StoragePath,
            UserId = input.UserId
        };
        ; 
        await _service.Update(asset);
        return RedirectToAction("Index");
    }
    // DELETE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var asset = await _service.GetById(id);
        var isAdmin = User.IsInRole("Admin");
        if (asset == null || (asset.UserId != userId && !isAdmin))
            return Json(new { success = false, message = "Asset not found or access denied" });

        // Delete file from disk
        var filePath = Path.Combine(_environment.WebRootPath, asset.StoragePath.TrimStart('/'));
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        await _service.Delete(id);
        return RedirectToAction("Index");
    }
}
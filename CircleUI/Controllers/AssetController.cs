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

[Authorize(Roles = "Admin")]
public class AssetController : Controller
{
    private readonly IAssetService _service;
    private readonly UserManager<User> _userManager;

    public AssetController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _service = new AssetService(context);
        _userManager = userManager;
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var assets = await _service.GetAll();
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
    public async Task<IActionResult> Delete(string id)
    {
        await _service.Delete(id);
        return RedirectToAction("Index");
    }
}
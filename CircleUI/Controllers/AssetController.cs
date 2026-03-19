using CircleUI.Core.Interfaces;
using CircleUI.Core.Services;
using CircleUI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Diagnostics;

namespace CircleUI.Controllers;

public class AssetController : Controller
{
    private readonly IAssetService _service;

    public AssetController(ApplicationDbContext context)
    {
        _service = new AssetService(context);
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var assets = await _service.GetAll();
        return View(assets);
    }
}
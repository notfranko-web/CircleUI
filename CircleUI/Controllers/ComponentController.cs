using CircleUI.Core.Interfaces;
using CircleUI.Core.Services;
using CircleUI.Data;
using Microsoft.AspNetCore.Mvc;

namespace CircleUI.Controllers;

public class ComponentController : Controller
{
    private readonly IComponentService _service;

    public ComponentController(ApplicationDbContext context)
    {
        _service = new ComponentService(context);
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var components = await _service.GetAll();
        return View(components);
    }
}
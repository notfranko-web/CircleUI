using CircleUI.Core.Interfaces;
using CircleUI.Core.Services;
using CircleUI.Data;
using Microsoft.AspNetCore.Mvc;

namespace CircleUI.Controllers;

public class PublishedVersionController : Controller
{
    private readonly IPublishedVersionService _service;

    public PublishedVersionController(ApplicationDbContext context)
    {
        _service = new PublishedVersionService(context);
    }
    // GET
    public IActionResult Index()
    {
        var versions = _service.GetAll();
        return View(versions);
    }
}
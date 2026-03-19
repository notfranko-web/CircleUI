using CircleUI.Core.Interfaces;
using CircleUI.Core.Services;
using CircleUI.Data;
using Microsoft.AspNetCore.Mvc;

namespace CircleUI.Controllers;

public class PageController : Controller
{
    private readonly IPageService _service;

    public PageController(ApplicationDbContext context)
    {
        _service = new PageService(context);
    }
    // GET
    public IActionResult Index()
    {
        var pages = _service.GetAll();
        return View(pages);
    }
}
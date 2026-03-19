using CircleUI.Core.Services;
using CircleUI.Data;
using Microsoft.AspNetCore.Mvc;

namespace CircleUI.Controllers;

public class WebsiteProjectController : Controller
{
    private readonly WebsiteProjectService _service;

    public WebsiteProjectController(ApplicationDbContext context)
    {
        _service = new WebsiteProjectService(context);
    }
    // GET
    public IActionResult Index()
    {
        var projects = _service.GetAll();
        return View(projects);
    }
}
using CircleUI.Core.Interfaces;
using CircleUI.Core.Services;
using CircleUI.Data;
using Microsoft.AspNetCore.Mvc;

namespace CircleUI.Controllers;

public class PublishedSiteController : Controller
{
    private readonly IWebsiteProjectService _service;

    public PublishedSiteController(ApplicationDbContext context)
    {
        _service = new WebsiteProjectService(context);
    }

    [Route("sites/{projectId}")]
    [Route("sites/{projectId}/{**pagePath}")]
    public async Task<IActionResult> Index(string projectId, string? pagePath = null)
    {
        var project = await _service.GetById(projectId);
        if (project == null || !project.IsPublished) return NotFound();

        var page = pagePath is null or ""
            ? project.Pages.FirstOrDefault()
            : project.Pages.FirstOrDefault(p =>
                p.Path.TrimStart('/').Equals(pagePath.TrimStart('/'), StringComparison.OrdinalIgnoreCase));

        if (page == null) return NotFound();

        ViewBag.Project = project;
        return View("PublishedSite", page);
    }
}

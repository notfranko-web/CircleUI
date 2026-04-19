using System.Security.Claims;
using CircleUI.Core.DTOs;
using CircleUI.Core.Interfaces;
using CircleUI.Core.Services;
using CircleUI.Data;
using CircleUI.ViewModels.WebsiteProject;
using Microsoft.AspNetCore.Mvc;

namespace CircleUI.Controllers;

public class WebsiteProjectController : Controller
{
    private readonly IWebsiteProjectService _service;
    private readonly IComponentService _componentService;

    public WebsiteProjectController(ApplicationDbContext context)
    {
        _service = new WebsiteProjectService(context);
        _componentService = new ComponentService(context);
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var projects = userId is not null
            ? await _service.GetByUserId(userId)
            : new List<WebsiteProjectDTO>();
        return View(projects);
    }
    
    // CREATE
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var websiteProject = new WebsiteProjectCreateViewModel();
        return View(websiteProject);
    }

    [HttpPost]
    public async Task<IActionResult> Create(WebsiteProjectCreateViewModel input)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(input);
        }
        WebsiteProjectDTO websiteProject = new WebsiteProjectDTO()
        {
            Name = input.Name,
            Description = input.Description,
            Domain = input.Domain,
            IsPublished = input.IsPublished,
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!
        };
        await _service.Create(websiteProject);
        return RedirectToAction("Index");
    }
    
    // EDIT
    public async Task<IActionResult> Edit(string id)
    {
        WebsiteProjectDTO existing = await _service.GetById(id);
        WebsiteProjectUpdateViewModel model = new WebsiteProjectUpdateViewModel()
        {
            Id = existing.Id ?? Guid.Empty,
            Name = existing.Name,
            Description = existing.Description,
            Domain = existing.Domain,
            IsPublished = existing.IsPublished,
            UserId = existing.UserId
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(WebsiteProjectUpdateViewModel input)
    {
        if (!ModelState.IsValid) return View(input);

        WebsiteProjectDTO websiteProject = new WebsiteProjectDTO()
        {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
            Domain = input.Domain,
            IsPublished = input.IsPublished,
            UserId = input.UserId
        };
        await _service.Update(websiteProject);
        return RedirectToAction("Index");
    }
    
    // DUPLICATE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Duplicate(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Unauthorized();
        await _service.Duplicate(id, userId);
        return RedirectToAction("Index");
    }

    // DELETE
    public async Task<IActionResult> Delete(string id)
    {
        await _service.Delete(id);
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Details(string id)
    {
        WebsiteProjectDTO websiteProject = await _service.GetById(id);
        var components = _componentService.GetAll();
        WebsiteProjectBuilderViewModel builder = new WebsiteProjectBuilderViewModel()
        {
            Name = websiteProject.Name,
            Description = websiteProject.Description,
            Domain = websiteProject.Domain,
            IsPublished = websiteProject.IsPublished,
            UserId = websiteProject.UserId,
            
        };
        return View(builder);
    }
    
    public async Task<IActionResult> Builder(string id, string? activeTab = null)
    {
        WebsiteProjectDTO websiteProject = await _service.EnsureHeaderFooter(id);
        var components = await _componentService.GetAll();
        WebsiteProjectBuilderViewModel builder = new WebsiteProjectBuilderViewModel()
        {
            Id = websiteProject.Id ?? Guid.Empty,
            Name = websiteProject.Name,
            Description = websiteProject.Description,
            Domain = websiteProject.Domain,
            IsPublished = websiteProject.IsPublished,
            UserId = websiteProject.UserId,
            BackgroundColor = websiteProject.BackgroundColor,
            PrimaryTextColor = websiteProject.PrimaryTextColor,
            SecondaryTextColor = websiteProject.SecondaryTextColor,
            ButtonColor = websiteProject.ButtonColor,
            ButtonTextColor = websiteProject.ButtonTextColor,
            BackgroundImage = websiteProject.BackgroundImage,
            PageDtos = websiteProject.Pages,
            ComponentDtos = components,
            HeaderSection = websiteProject.HeaderSection,
            FooterSection = websiteProject.FooterSection,
            ActiveTab = activeTab
        };
        return View(builder);
    }

    public async Task<IActionResult> Preview(string id, Guid pageId)
    {
        WebsiteProjectDTO websiteProject = await _service.GetById(id);
        var page = websiteProject.Pages.FirstOrDefault(p => p.Id == pageId);
        if (page is null) return NotFound();
        
        ViewBag.Project = websiteProject;
        return View(page);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateTheme([FromForm] Guid projectId, [FromForm] string backgroundColor, [FromForm] string primaryTextColor, [FromForm] string secondaryTextColor, [FromForm] string buttonColor, [FromForm] string buttonTextColor, [FromForm] string? backgroundImage)
    {
        var project = await _service.GetById(projectId.ToString());
        if (project == null) return NotFound(new { success = false, message = "Project not found" });

        project.BackgroundColor = backgroundColor;
        project.PrimaryTextColor = primaryTextColor;
        project.SecondaryTextColor = secondaryTextColor;
        project.ButtonColor = buttonColor;
        project.ButtonTextColor = buttonTextColor;
        project.BackgroundImage = backgroundImage;

        await _service.Update(project);

        return Json(new { success = true });
    }
}
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
        var projects = await _service.GetAll();
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
            UserId = input.UserId
        }; 
        await _service.Create(websiteProject);
        return RedirectToAction("Index");
    }
    
    // UPDATE
    public async Task<IActionResult> Update(string id)
    {
        WebsiteProjectDTO existing = await _service.GetById(id);
        WebsiteProjectUpdateViewModel model = new WebsiteProjectUpdateViewModel()
        {
            Name = existing.Name,
            Description = existing.Description,
            Domain = existing.Domain,
            IsPublished = existing.IsPublished,
            UserId = existing.UserId
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(WebsiteProjectUpdateViewModel input)
    {
        WebsiteProjectDTO websiteProject = new WebsiteProjectDTO()
        {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
            Domain = input.Domain,
            IsPublished = input.IsPublished,
            UserId = input.UserId
        };
        ; 
        await _service.Update(websiteProject);
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
    
    public async Task<IActionResult> Builder(string id)
    {
        WebsiteProjectDTO websiteProject = await _service.GetById(id);
        var components = await _componentService.GetAll();
        WebsiteProjectBuilderViewModel builder = new WebsiteProjectBuilderViewModel()
        {
            Name = websiteProject.Name,
            Description = websiteProject.Description,
            Domain = websiteProject.Domain,
            IsPublished = websiteProject.IsPublished,
            UserId = websiteProject.UserId,
            PageDtos  = websiteProject.Pages,
            ComponentDtos = components
        };
        return View(builder);
    }
}
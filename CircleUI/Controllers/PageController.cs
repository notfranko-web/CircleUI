using CircleUI.Core.DTOs;
using CircleUI.Core.Interfaces;
using CircleUI.Core.Services;
using CircleUI.Data;
using CircleUI.ViewModels.Page;
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
    public async Task<IActionResult> Index()
    {
        var pages = await _service.GetAll();
        return View(pages);
    }
    
    // CREATE
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var page = new PageCreateViewModel();
        return View(page);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PageCreateViewModel input)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(input);
        }
        PageDTO page = new PageDTO()
        {
            Title = input.Title,
            Path = input.Path,
            MetaDescription = input.MetaDescription,
            MetaKeywords = input.MetaKeywords,
            ProjectId = input.ProjectId
        }; 
        await _service.Create(page);
        return RedirectToAction("Index");
    }
    
    // CREATE FROM BUILDER
    [HttpPost]
    public async Task<IActionResult> CreateForBuilder(Guid projectId, string title)
    {
        var page = new PageDTO()
        {
            Title = title,
            Path = "/" + Uri.EscapeDataString(title),
            ProjectId = projectId
        };
        try
        {
            await _service.Create(page);
        }
        catch (InvalidOperationException ex)
        {
            TempData["PageError"] = ex.Message;
        }
        return RedirectToAction("Builder", "WebsiteProject", new { id = projectId });
    }

    // UPDATE
    public async Task<IActionResult> Update(string id)
    {
        PageDTO existing = await _service.GetById(id);
        PageUpdateViewModel model = new PageUpdateViewModel()
        {
            Title = existing.Title,
            Path = existing.Path,
            MetaDescription = existing.MetaDescription,
            MetaKeywords = existing.MetaKeywords,
            ProjectId = existing.ProjectId
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(PageUpdateViewModel input)
    {
        PageDTO page = new PageDTO()
        {
            Id = input.Id,
            Title = input.Title,
            Path = input.Path,
            MetaDescription = input.MetaDescription,
            MetaKeywords = input.MetaKeywords,
            ProjectId = input.ProjectId
        };
        ; 
        await _service.Update(page);
        return RedirectToAction("Index");
    }
    
    // DELETE
    public async Task<IActionResult> Delete(string id)
    {
        await _service.Delete(id);
        return RedirectToAction("Index");
    }
}
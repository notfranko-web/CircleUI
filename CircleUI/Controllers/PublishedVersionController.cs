using CircleUI.Core.DTOs;
using CircleUI.Core.Interfaces;
using CircleUI.Core.Services;
using CircleUI.Data;
using CircleUI.ViewModels.PublishedVersion;
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
    public async Task<IActionResult> Index()
    {
        var versions = await _service.GetAll();
        return View(versions);
    }
    
    // CREATE
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var publishedVersion = new PublishedVersionCreateViewModel();
        return View(publishedVersion);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PublishedVersionCreateViewModel input)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(input);
        }
        PublishedVersionDTO publishedVersion = new PublishedVersionDTO()
        {
            PublishedAt = input.PublishedAt,
            VersionHash = input.VersionHash,
            ProjectId = input.ProjectId
        }; 
        await _service.Create(publishedVersion);
        return RedirectToAction("Index");
    }
    
    // UPDATE
    public async Task<IActionResult> Update(string id)
    {
        PublishedVersionDTO existing = await _service.GetById(id);
        PublishedVersionUpdateViewModel model = new PublishedVersionUpdateViewModel()
        {
            PublishedAt = existing.PublishedAt,
            VersionHash = existing.VersionHash,
            ProjectId = existing.ProjectId
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(PublishedVersionUpdateViewModel input)
    {
        PublishedVersionDTO publishedVersion = new PublishedVersionDTO()
        {
            Id = input.Id,
            PublishedAt = input.PublishedAt,
            VersionHash = input.VersionHash,
            ProjectId = input.ProjectId
        };
        ; 
        await _service.Update(publishedVersion);
        return RedirectToAction("Index");
    }
    
    // DELETE
    public async Task<IActionResult> Delete(string id)
    {
        await _service.Delete(id);
        return RedirectToAction("Index");
    }
}
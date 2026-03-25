using CircleUI.Core.DTOs;
using CircleUI.Core.Interfaces;
using CircleUI.Core.Services;
using CircleUI.Data;
using CircleUI.ViewModels.Component;
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
    // CREATE
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var component = new ComponentCreateViewModel();
        return View(component);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ComponentCreateViewModel input)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(input);
        }

        ComponentDTO component = new ComponentDTO()
        {
            Type = input.Type,
            Content = input.Content,
            Layout = input.Content,
            ParentId = input.ParentId,
            PageId = input.PageId
        };
        await _service.Create(component);
        return RedirectToAction("Index");
    }
    // UPDATE

    public async Task<IActionResult> Update(string id)
    {
        ComponentDTO existing = await _service.GetById(id);
        ComponentUpdateViewModel model = new ComponentUpdateViewModel()
        {
            Type = existing.Type,
            Content = existing.Content,
            Layout = existing.Content,
            ParentId = existing.ParentId,
            PageId = existing.PageId
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ComponentUpdateViewModel input)
    {
        ComponentDTO component = new ComponentDTO()
        {
            Type = input.Type,
            Content = input.Content,
            Layout = input.Content,
            ParentId = input.ParentId,
            PageId = input.PageId
        };
        await _service.Update(component);
        return RedirectToAction("Index");
    }
    
    //DELETE
    
    public async Task<IActionResult> Delete(string id)
    {
        await _service.Delete(id);
        return RedirectToAction("Index");
    }
}
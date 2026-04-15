using CircleUI.Data;
using CircleUI.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CircleUI.Controllers;

public class SectionController : Controller
{
    private readonly ApplicationDbContext _context;

    public SectionController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> UpdateComponentContent(Guid componentId, string content)
    {
        var component = await _context.Components.FindAsync(componentId);
        if (component is null) return NotFound();
        component.Content = content;
        await _context.SaveChangesAsync();
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> AddComponent(Guid sectionId, Guid componentId)
    {
        var order = await _context.SectionComponents.CountAsync(sc => sc.SectionId == sectionId);
        _context.SectionComponents.Add(new SectionComponent
        {
            SectionId = sectionId,
            ComponentId = componentId,
            Order = order
        });
        await _context.SaveChangesAsync();

        var component = await _context.Components.FindAsync(componentId);
        return Json(new { success = true, type = component!.Type, content = component.Content });
    }

    [HttpPost]
    public async Task<IActionResult> AddToPage(Guid pageId, Guid projectId, string name = "New Section")
    {
        var order = await _context.PageSections.CountAsync(ps => ps.PageId == pageId);

        var section = new Section { Name = name };
        _context.Sections.Add(section);

        var pageSection = new PageSection
        {
            PageId = pageId,
            SectionId = section.Id,
            Order = order
        };
        _context.PageSections.Add(pageSection);

        await _context.SaveChangesAsync();

        return RedirectToAction("Builder", "WebsiteProject", new { id = projectId, activeTab = $"pane-{pageId}" });
    }
}

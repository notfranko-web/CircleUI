using CircleUI.Core.DTOs;
using CircleUI.Core.Interfaces;
using CircleUI.Data;
using CircleUI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CircleUI.Core.Services;

public class PageService : IPageService
{
    private readonly ApplicationDbContext _context;
    public PageService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PageDTO>> GetAll()
    {
        var pages = await _context.Pages.ToListAsync();
        return pages.Select(p => new PageDTO()
        {
            Id = p.Id,
            Title = p.Title,
            Path = p.Path,
            MetaDescription = p.MetaDescription,
            MetaKeywords = p.MetaKeywords,
            ProjectId = p.ProjectId
        }).ToList();
    }

    public async Task<PageDTO> Create(PageDTO pages)
    {
        var order = await _context.Pages.CountAsync(p => p.ProjectId == pages.ProjectId);
        var page = new Page()
        {
            Title = pages.Title,
            Path = pages.Path,
            MetaDescription = pages.MetaDescription,
            MetaKeywords = pages.MetaKeywords,
            ProjectId = pages.ProjectId,
            Order = order
        };
        await _context.Pages.AddAsync(page);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new InvalidOperationException($"A page named \"{pages.Title}\" already exists in this project.");
        }

        return new PageDTO()
        {
            Id = page.Id,
            Title = page.Title,
            Path = page.Path,
            MetaDescription = page.MetaDescription,
            MetaKeywords = page.MetaKeywords,
            ProjectId = page.ProjectId
        };
    }
    
    public async Task<PageDTO> GetById(string id)
    {
        Guid guidId = new Guid(id);
        var page = await _context.Pages.FirstOrDefaultAsync(p => p.Id == guidId);
        return new PageDTO()
        {
            Id = page.Id,
            Title = page.Title,
            Path = page.Path,
            MetaDescription = page.MetaDescription,
            MetaKeywords = page.MetaKeywords,
            ProjectId = page.ProjectId
        };
    }

    public async Task<PageDTO> Update(PageDTO input)
    {
        var page = await _context.Pages.FirstOrDefaultAsync(p => p.Id == input.Id);
        page.Title = input.Title;
        page.Path = input.Path;
        page.MetaDescription = input.MetaDescription;
        page.MetaKeywords = input.MetaKeywords;
        page.ProjectId = input.ProjectId;
        await _context.SaveChangesAsync();

        return new PageDTO()
        {
            Id = page.Id,
            Title = page.Title,
            Path = page.Path,
            MetaDescription = page.MetaDescription,
            MetaKeywords = page.MetaKeywords,
            ProjectId = page.ProjectId
        };
    }
    
    public async Task Delete(string id)
    {
        Guid guidId = new Guid(id);
        var page = _context.Pages.FirstOrDefault(p => p.Id == guidId);
        _context.Pages.Remove(page);
        await _context.SaveChangesAsync();
    }
}
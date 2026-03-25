using CircleUI.Core.DTOs;
using CircleUI.Core.Interfaces;
using CircleUI.Data;
using CircleUI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CircleUI.Core.Services;

public class WebsiteProjectService : IWebsiteProjectService
{
    private readonly ApplicationDbContext _context;
    
    public WebsiteProjectService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<WebsiteProject>> GetAll()
    {
        return await _context.WebsiteProjects.ToListAsync();
    }

    public async Task<WebsiteProject> Create(WebsiteProjectDTO input)
    {
        var websiteProject = new WebsiteProject()
        {
            Name = input.Name,
            Description = input.Description,
            Domain = input.Domain,
            IsPublished = input.IsPublished,
            UserId = input.UserId
        };
        await _context.WebsiteProjects.AddAsync(websiteProject);
        await _context.SaveChangesAsync();
        return websiteProject;
    }
    
    public async Task<WebsiteProjectDTO> GetById(string id)
    {
        Guid guidId = new Guid(id);
        var websiteProject = await _context.WebsiteProjects.FirstOrDefaultAsync(w => w.Id == guidId);
        
        return new WebsiteProjectDTO()
        {
            Name = websiteProject.Name,
            Description = websiteProject.Description,
            Domain = websiteProject.Domain,
            IsPublished = websiteProject.IsPublished,
            UserId = websiteProject.UserId
        }; 
    }
    
    public async Task<WebsiteProject> Update(WebsiteProjectDTO input)
    {
        var websiteProject = await _context.WebsiteProjects.FirstOrDefaultAsync(w => w.Id == input.Id);
        websiteProject.Name = input.Name;
        websiteProject.Description = input.Description;
        websiteProject.Domain = input.Domain;
        websiteProject.IsPublished = input.IsPublished;
        websiteProject.UserId = input.UserId;
        await _context.SaveChangesAsync();
        return websiteProject;
    }
    
    public async Task Delete(string id)
    {
        Guid guidId = new Guid(id);
        var websiteProject = await _context.WebsiteProjects.FirstOrDefaultAsync(w => w.Id == guidId);
        _context.WebsiteProjects.Remove(websiteProject);
        await _context.SaveChangesAsync();
    }
}
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

    public async Task<List<WebsiteProjectDTO>> GetAll()
    {
        var websiteProjects = await _context.WebsiteProjects.ToListAsync();
        return websiteProjects.Select(wp => new WebsiteProjectDTO()
        {
            Id = wp.Id,
            Name = wp.Name,
            Description = wp.Description,
            Domain = wp.Domain,
            IsPublished = wp.IsPublished,
            UserId = wp.UserId
        }).ToList();
    }

    public async Task<WebsiteProjectDTO> Create(WebsiteProjectDTO input)
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

        return new WebsiteProjectDTO()
        {
            Id = websiteProject.Id,
            Name = websiteProject.Name,
            Description = websiteProject.Description,
            Domain = websiteProject.Domain,
            IsPublished = websiteProject.IsPublished,
            UserId = websiteProject.UserId
        };
    }
    
    public async Task<WebsiteProjectDTO> GetById(string id)
    {
        Guid guidId = new Guid(id);
        //include pages
        var websiteProject = await _context.WebsiteProjects
            .Include(w => w.Pages)
            .ThenInclude(p => p.PageSections)
            .ThenInclude(ps => ps.Section)
            .ThenInclude(s => s.SectionComponents)
            .ThenInclude(sc => sc.Component)
            .FirstOrDefaultAsync(w => w.Id == guidId);

        var output =  new WebsiteProjectDTO()
        {
            Id = websiteProject.Id,
            Name = websiteProject.Name,
            Description = websiteProject.Description,
            Domain = websiteProject.Domain,
            IsPublished = websiteProject.IsPublished,
            UserId = websiteProject.UserId,
            Pages = new List<PageDTO>(),
            
        };

        foreach (var page in websiteProject.Pages)
        {
            var dto = new PageDTO()
            {
                Id = page.Id,
                Title = page.Title,
                Path = page.Path,
                MetaDescription = page.MetaDescription,
                MetaKeywords = page.MetaKeywords,
                ProjectId = page.ProjectId 
            };
            foreach (var section in page.PageSections)
            {
                var sectionDto = new SectionDTO()
                {
                    Id = section.SectionId,
                    Name = section.Section.Name,
                    HTMLId = section.Section.HTMLId,
                    CSSClass = section.Section.CSSClass,
                };

                foreach (var component in section.Section.SectionComponents)
                {
                    var componentDto = new ComponentDTO()
                    {
                        Id = component.ComponentId,
                        Type = component.Component.Type,
                        Content = component.Component.Content,
                        Layout = component.Component.Layout,
                    };
                    sectionDto.ComponentDTOs.Add(componentDto);
                }
                
                dto.SectionDtos.Add(sectionDto);
            }
            output.Pages.Add(dto);
        }
        
        
        return output;
    }

    public async Task<WebsiteProjectDTO> Update(WebsiteProjectDTO input)
    {
        var websiteProject = await _context.WebsiteProjects.FirstOrDefaultAsync(w => w.Id == input.Id);
        websiteProject.Name = input.Name;
        websiteProject.Description = input.Description;
        websiteProject.Domain = input.Domain;
        websiteProject.IsPublished = input.IsPublished;
        websiteProject.UserId = input.UserId;
        await _context.SaveChangesAsync();

        return new WebsiteProjectDTO()
        {
            Id = websiteProject.Id,
            Name = websiteProject.Name,
            Description = websiteProject.Description,
            Domain = websiteProject.Domain,
            IsPublished = websiteProject.IsPublished,
            UserId = websiteProject.UserId
        };
    }
    
    public async Task Delete(string id)
    {
        Guid guidId = new Guid(id);
        var websiteProject = await _context.WebsiteProjects.FirstOrDefaultAsync(w => w.Id == guidId);
        _context.WebsiteProjects.Remove(websiteProject);
        await _context.SaveChangesAsync();
    }
}
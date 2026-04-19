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
            UserId = wp.UserId,
            BackgroundColor = wp.BackgroundColor,
            PrimaryTextColor = wp.PrimaryTextColor,
            SecondaryTextColor = wp.SecondaryTextColor,
            ButtonColor = wp.ButtonColor,
            ButtonTextColor = wp.ButtonTextColor
        }).ToList();
    }

    public async Task<List<WebsiteProjectDTO>> GetByUserId(string userId)
    {
        var websiteProjects = await _context.WebsiteProjects
            .Where(wp => wp.UserId == userId)
            .ToListAsync();
        return websiteProjects.Select(wp => new WebsiteProjectDTO()
        {
            Id = wp.Id,
            Name = wp.Name,
            Description = wp.Description,
            Domain = wp.Domain,
            IsPublished = wp.IsPublished,
            UserId = wp.UserId,
            BackgroundColor = wp.BackgroundColor,
            PrimaryTextColor = wp.PrimaryTextColor,
            SecondaryTextColor = wp.SecondaryTextColor,
            ButtonColor = wp.ButtonColor,
            ButtonTextColor = wp.ButtonTextColor
        }).ToList();
    }

    public async Task<WebsiteProjectDTO> EnsureHeaderFooter(string projectId)
    {
        Guid guidId = new Guid(projectId);
        var project = await _context.WebsiteProjects.FirstOrDefaultAsync(w => w.Id == guidId);
        if (project == null) throw new Exception("Project not found");

        bool changed = false;
        if (project.HeaderSectionId == null)
        {
            var header = new Section { Name = "Global Header" };
            _context.Sections.Add(header);
            await _context.SaveChangesAsync();
            project.HeaderSectionId = header.Id;
            changed = true;
        }
        if (project.FooterSectionId == null)
        {
            var footer = new Section { Name = "Global Footer" };
            _context.Sections.Add(footer);
            await _context.SaveChangesAsync();
            project.FooterSectionId = footer.Id;
            changed = true;
        }
        if (changed) await _context.SaveChangesAsync();

        return await GetById(projectId);
    }

    public async Task<WebsiteProjectDTO> Create(WebsiteProjectDTO input)
    {
        var headerSection = new Section { Name = "Global Header" };
        var footerSection = new Section { Name = "Global Footer" };
        _context.Sections.AddRange(headerSection, footerSection);
        await _context.SaveChangesAsync();

        var websiteProject = new WebsiteProject()
        {
            Name = input.Name,
            Description = input.Description,
            Domain = input.Domain,
            IsPublished = input.IsPublished,
            UserId = input.UserId,
            BackgroundColor = input.BackgroundColor,
            PrimaryTextColor = input.PrimaryTextColor,
            SecondaryTextColor = input.SecondaryTextColor,
            ButtonColor = input.ButtonColor,
            ButtonTextColor = input.ButtonTextColor,
            HeaderSectionId = headerSection.Id,
            FooterSectionId = footerSection.Id
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
            UserId = websiteProject.UserId,
            BackgroundColor = websiteProject.BackgroundColor,
            PrimaryTextColor = websiteProject.PrimaryTextColor,
            SecondaryTextColor = websiteProject.SecondaryTextColor,
            ButtonColor = websiteProject.ButtonColor,
            ButtonTextColor = websiteProject.ButtonTextColor,
            BackgroundImage = websiteProject.BackgroundImage
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
            .Include(w => w.HeaderSection)
            .ThenInclude(s => s.SectionComponents)
            .ThenInclude(sc => sc.Component)
            .Include(w => w.FooterSection)
            .ThenInclude(s => s.SectionComponents)
            .ThenInclude(sc => sc.Component)
            .FirstOrDefaultAsync(w => w.Id == guidId);

        var output = new WebsiteProjectDTO()
        {
            Id = websiteProject.Id,
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
            HeaderSection = websiteProject.HeaderSection != null ? MapSection(websiteProject.HeaderSection) : null,
            FooterSection = websiteProject.FooterSection != null ? MapSection(websiteProject.FooterSection) : null,
            Pages = new List<PageDTO>(),
        };

        foreach (var page in websiteProject.Pages.OrderBy(p => p.Order).ThenBy(p => p.CreatedAt))
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
            foreach (var section in page.PageSections.OrderBy(ps => ps.Order))
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
                        SectionComponentId = component.Id,
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
        websiteProject.BackgroundColor = input.BackgroundColor;
        websiteProject.PrimaryTextColor = input.PrimaryTextColor;
        websiteProject.SecondaryTextColor = input.SecondaryTextColor;
        websiteProject.ButtonColor = input.ButtonColor;
        websiteProject.ButtonTextColor = input.ButtonTextColor;
        websiteProject.BackgroundImage = input.BackgroundImage;
        await _context.SaveChangesAsync();

        return new WebsiteProjectDTO()
        {
            Id = websiteProject.Id,
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
            BackgroundImage = websiteProject.BackgroundImage
        };
    }
    
    public async Task Delete(string id)
    {
        Guid guidId = new Guid(id);
        var websiteProject = await _context.WebsiteProjects.FirstOrDefaultAsync(w => w.Id == guidId);
        _context.WebsiteProjects.Remove(websiteProject);
        await _context.SaveChangesAsync();
    }

    public async Task<WebsiteProjectDTO> Duplicate(string projectId, string userId)
    {
        Guid guidId = new Guid(projectId);
        var original = await _context.WebsiteProjects
            .Include(w => w.Pages)
                .ThenInclude(p => p.PageSections)
                .ThenInclude(ps => ps.Section)
                .ThenInclude(s => s.SectionComponents)
                .ThenInclude(sc => sc.Component)
            .Include(w => w.HeaderSection)
                .ThenInclude(s => s.SectionComponents)
                .ThenInclude(sc => sc.Component)
            .Include(w => w.FooterSection)
                .ThenInclude(s => s.SectionComponents)
                .ThenInclude(sc => sc.Component)
            .FirstOrDefaultAsync(w => w.Id == guidId);

        if (original == null) throw new Exception("Project not found");

        Section CopySection(Section src)
        {
            var newSection = new Section { Name = src.Name, HTMLId = src.HTMLId, CSSClass = src.CSSClass };
            foreach (var sc in src.SectionComponents.OrderBy(x => x.Order))
            {
                var newComponent = new Component
                {
                    Type = sc.Component.Type,
                    Category = sc.Component.Category,
                    Content = sc.Component.Content,
                    Layout = sc.Component.Layout,
                    IsTemplate = false
                };
                newSection.SectionComponents.Add(new SectionComponent
                {
                    Component = newComponent,
                    Order = sc.Order
                });
            }
            return newSection;
        }

        var newHeader = original.HeaderSection != null ? CopySection(original.HeaderSection) : new Section { Name = "Global Header" };
        var newFooter = original.FooterSection != null ? CopySection(original.FooterSection) : new Section { Name = "Global Footer" };
        _context.Sections.AddRange(newHeader, newFooter);

        var copy = new WebsiteProject
        {
            Name = original.Name + " (Copy)",
            Description = original.Description,
            Domain = string.Empty,
            IsPublished = false,
            UserId = userId,
            BackgroundColor = original.BackgroundColor,
            PrimaryTextColor = original.PrimaryTextColor,
            SecondaryTextColor = original.SecondaryTextColor,
            ButtonColor = original.ButtonColor,
            ButtonTextColor = original.ButtonTextColor,
            BackgroundImage = original.BackgroundImage,
            HeaderSection = newHeader,
            FooterSection = newFooter
        };

        foreach (var page in original.Pages.OrderBy(p => p.CreatedAt))
        {
            var newPage = new Page
            {
                Title = page.Title,
                Path = page.Path,
                MetaDescription = page.MetaDescription,
                MetaKeywords = page.MetaKeywords
            };
            int order = 0;
            foreach (var ps in page.PageSections.OrderBy(ps => ps.Order))
            {
                var newSection = CopySection(ps.Section);
                newPage.PageSections.Add(new PageSection { Section = newSection, Order = order++ });
            }
            copy.Pages.Add(newPage);
        }

        await _context.WebsiteProjects.AddAsync(copy);
        await _context.SaveChangesAsync();

        return await GetById(copy.Id.ToString());
    }

    private SectionDTO MapSection(Section section)
    {
        var dto = new SectionDTO
        {
            Id = section.Id,
            Name = section.Name,
            HTMLId = section.HTMLId ?? string.Empty,
            CSSClass = section.CSSClass ?? string.Empty,
        };
        foreach (var sc in section.SectionComponents.OrderBy(x => x.Order))
        {
            dto.ComponentDTOs.Add(new ComponentDTO
            {
                Id = sc.Component.Id,
                SectionComponentId = sc.Id,
                Type = sc.Component.Type,
                Content = sc.Component.Content,
                Layout = sc.Component.Layout,
            });
        }
        return dto;
    }
}
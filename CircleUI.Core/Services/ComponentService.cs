using CircleUI.Core.DTOs;
using CircleUI.Core.Interfaces;
using CircleUI.Data;
using CircleUI.Data.Models;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace CircleUI.Core.Services;

public class ComponentService : IComponentService
{
    private readonly ApplicationDbContext _context;
    
    public ComponentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ComponentDTO>> GetAll()
    {
        var components = await _context.Components.ToListAsync();
        var output = new List<ComponentDTO>();
        foreach (var component in components)
        {
            var dto = new ComponentDTO()
            {
                Id = component.Id,
                Type = component.Type,
                Category = component.Category,
                Content = component.Content,
                Layout = component.Layout,
            };
            output.Add(dto);
        }
        return output;
    }

    public async Task<ComponentDTO> Create(ComponentDTO input)
    {
        var component = new Component()
        {
            Type = input.Type,
            Content = input.Content,
            Layout = input.Layout,
        };
        await _context.Components.AddAsync(component);
        await _context.SaveChangesAsync();

        return new ComponentDTO()
        {
            Id = component.Id,
            Type = component.Type,
            Content = component.Content,
            Layout = component.Layout,
        };
    }
    
    public async Task<ComponentDTO> GetById(string id)
    {
        Guid guidId = new Guid(id);
        var component = await _context.Components.FirstOrDefaultAsync(c => c.Id == guidId);

        return new ComponentDTO()
        {
            Id = component.Id,
            Type = component.Type,
            Content = component.Content,
            Layout = component.Layout,
        };
    }
    
    public async Task<ComponentDTO> Update(ComponentDTO input)
    {
        var component = await _context.Components.FirstOrDefaultAsync(c => c.Id == input.Id);
        component.Type = input.Type;
        component.Content = input.Content;
        component.Layout = input.Layout;
        await _context.SaveChangesAsync();

        return new ComponentDTO()
        {
            Id = component.Id,
            Type = component.Type,
            Content = component.Content,
            Layout = component.Layout,
        };
    }
    
    public async Task Delete(string id)
    {
        Guid guidId = new Guid(id);
        var component = await _context.Components.FirstOrDefaultAsync(c => c.Id == guidId);
        _context.Components.Remove(component);
        await _context.SaveChangesAsync();
    }
}
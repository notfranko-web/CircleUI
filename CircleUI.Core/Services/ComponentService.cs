using CircleUI.Core.DTOs;
using CircleUI.Core.Interfaces;
using CircleUI.Data;
using CircleUI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CircleUI.Core.Services;

public class ComponentService : IComponentService
{
    private readonly ApplicationDbContext _context;
    
    public ComponentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Component>> GetAll()
    {
        return await _context.Components.ToListAsync();
    }

    public async Task<Component> Create(ComponentDTO input)
    {
        var component = new Component()
        {
            Type = input.Type,
            Content = input.Content,
            Layout = input.Layout,
            ParentId = input.ParentId,
            PageId = input.PageId
        };
        await _context.Components.AddAsync(component);
        await _context.SaveChangesAsync();
        return component;
    }
    
    public async Task<ComponentDTO> GetById(string id)
    {
        Guid guidId = new Guid(id);
        var component = await _context.Components.FirstOrDefaultAsync(c => c.Id == guidId);
        
        return new ComponentDTO()
        {
            Type = component.Type,
            Content = component.Content,
            Layout = component.Layout,
            ParentId = component.ParentId,
            PageId = component.PageId
        }; 
    }
    
    public async Task<Component> Update(ComponentDTO input)
    {
        var component = await _context.Components.FirstOrDefaultAsync(c => c.Id == input.Id);
        component.Type = input.Type;
        component.Content = input.Content;
        component.Layout = input.Layout;
        component.ParentId = input.ParentId;
        component.PageId = input.PageId;
        await _context.SaveChangesAsync();
        return component;
    }
    
    public async Task Delete(string id)
    {
        Guid guidId = new Guid(id);
        var component = await _context.Components.FirstOrDefaultAsync(c => c.Id == guidId);
        _context.Components.Remove(component);
        await _context.SaveChangesAsync();
    }
}
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
}
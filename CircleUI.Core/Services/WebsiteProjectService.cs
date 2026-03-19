using CircleUI.Data;
using CircleUI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CircleUI.Core.Services;

public class WebsiteProjectService
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
}
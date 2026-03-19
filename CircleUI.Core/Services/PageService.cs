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

    public async Task<List<Page>> GetAll()
    {
        return await _context.Pages.ToListAsync();
    }
}
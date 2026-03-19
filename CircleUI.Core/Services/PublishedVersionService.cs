using CircleUI.Core.Interfaces;
using CircleUI.Data;
using CircleUI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CircleUI.Core.Services;

public class PublishedVersionService : IPublishedVersionService
{
    private readonly ApplicationDbContext _context;
    public PublishedVersionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PublishedVersion>> GetAll()
    {
        return await _context.PublishedVersions.ToListAsync();
    }
}
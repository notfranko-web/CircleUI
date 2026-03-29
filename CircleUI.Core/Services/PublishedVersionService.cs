using CircleUI.Core.DTOs;
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
    
    public async Task<List<PublishedVersionDTO>> GetAll()
    {
        var publishedVersions = await _context.PublishedVersions.ToListAsync();
        return publishedVersions.Select(pv => new PublishedVersionDTO()
        {
            Id = pv.Id,
            PublishedAt = pv.PublishedAt,
            VersionHash = pv.VersionHash,
            ProjectId = pv.ProjectId
        }).ToList();
    }

    public async Task<PublishedVersionDTO> Create(PublishedVersionDTO input)
    {
        var publishedVersion = new PublishedVersion()
        {
            PublishedAt = input.PublishedAt,
            VersionHash = input.VersionHash,
            ProjectId = input.ProjectId
        };
        await _context.PublishedVersions.AddAsync(publishedVersion);
        await _context.SaveChangesAsync();

        return new PublishedVersionDTO()
        {
            Id = publishedVersion.Id,
            PublishedAt = publishedVersion.PublishedAt,
            VersionHash = publishedVersion.VersionHash,
            ProjectId = publishedVersion.ProjectId
        };
    }
    
    public async Task<PublishedVersionDTO> GetById(string id)
    {
        Guid guidId = new Guid(id);
        var publishedVersion = await _context.PublishedVersions.FirstOrDefaultAsync(p => p.Id == guidId);

        return new PublishedVersionDTO()
        {
            PublishedAt = publishedVersion.PublishedAt,
            VersionHash = publishedVersion.VersionHash,
            ProjectId = publishedVersion.ProjectId
        };
    }

    public async Task<PublishedVersionDTO> Update(PublishedVersionDTO input)
    {
        var publishedVersion = await _context.PublishedVersions.FirstOrDefaultAsync(p => p.Id == input.Id);
        publishedVersion.PublishedAt = input.PublishedAt;
        publishedVersion.VersionHash = input.VersionHash;
        publishedVersion.ProjectId = input.ProjectId;
        await _context.SaveChangesAsync();

        return new PublishedVersionDTO()
        {
            Id = publishedVersion.Id,
            PublishedAt = publishedVersion.PublishedAt,
            VersionHash = publishedVersion.VersionHash,
            ProjectId = publishedVersion.ProjectId
        };
    }
    
    public async Task Delete(string id)
    {
        Guid guidId = new Guid(id);
        var publishedVersion = await _context.PublishedVersions.FirstOrDefaultAsync(p => p.Id == guidId);
        _context.PublishedVersions.Remove(publishedVersion);
        await _context.SaveChangesAsync();
    }
}
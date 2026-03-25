using CircleUI.Core.DTOs;
using CircleUI.Core.Interfaces;
using CircleUI.Data;
using CircleUI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CircleUI.Core.Services;

public class AssetService : IAssetService
{
    
    private readonly ApplicationDbContext _context;
    
    public AssetService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Asset>> GetAll()
    {
        //throw new NotImplementedException();
        return  await _context.Assets.ToListAsync();
    }

    public async Task<Asset> Create(AssetDTO input)
    {
        var asset = new Asset()
        {
            FileName = input.FileName,
            MimeType = input.MimeType,
            SizeBytes = input.SizeBytes,
            StoragePath = input.StoragePath,
            UserId = input.UserId
        };
        await _context.Assets.AddAsync(asset);
        await _context.SaveChangesAsync();
        return asset;
    }
    
    public async Task<AssetDTO> GetById(string id)
    {
        Guid guidId = new Guid(id);
        var asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == guidId);
        
        return new AssetDTO()
        {
            FileName = asset.FileName,
            MimeType = asset.MimeType,
            SizeBytes = asset.SizeBytes,
            StoragePath = asset.StoragePath,
            UserId = asset.UserId
        }; 
    }
    
    public async Task<Asset> Update(AssetDTO input)
    {
        var asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == input.Id);
        asset.FileName = input.FileName;
        asset.MimeType = input.MimeType;
        asset.SizeBytes = input.SizeBytes;
        asset.StoragePath = input.StoragePath;
        asset.UserId = input.UserId;
        await _context.SaveChangesAsync();
        return asset;
    }
    
    public async Task Delete(string id)
    {
        Guid guidId = new Guid(id);
        var asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == guidId);
        _context.Assets.Remove(asset);
        await _context.SaveChangesAsync();
    }
    
}
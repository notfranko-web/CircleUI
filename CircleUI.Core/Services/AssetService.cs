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
}
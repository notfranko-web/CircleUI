using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IAssetService
{
    public Task<List<Asset>> GetAll();
    
}
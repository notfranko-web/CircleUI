using CircleUI.Core.DTOs;
using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IAssetService
{
    public Task<List<Asset>> GetAll();
    public Task<Asset> Create(AssetDTO assets);
    public Task<AssetDTO> GetById(string id);
    public Task<Asset> Update(AssetDTO assets);
    public Task Delete(string id);
}
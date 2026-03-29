using CircleUI.Core.DTOs;
using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IAssetService
{
    public Task<List<AssetDTO>> GetAll();
    public Task<AssetDTO> Create(AssetDTO assets);
    public Task<AssetDTO> GetById(string id);
    public Task<AssetDTO> Update(AssetDTO assets);
    public Task Delete(string id);
}
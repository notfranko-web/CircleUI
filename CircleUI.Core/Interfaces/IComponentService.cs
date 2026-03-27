using CircleUI.Core.DTOs;
using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IComponentService
{
    public Task<List<ComponentDTO>> GetAll();
    public Task<ComponentDTO> Create(ComponentDTO components);
    public Task<ComponentDTO> GetById(string id);
    public Task<ComponentDTO> Update(ComponentDTO assets);
    public Task Delete(string id);
}

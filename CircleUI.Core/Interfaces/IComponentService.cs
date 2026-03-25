using CircleUI.Core.DTOs;
using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IComponentService
{
    public Task<List<Component>> GetAll();
    public Task<Component> Create(ComponentDTO components);
    public Task<ComponentDTO> GetById(string id);
    public Task<Component> Update(ComponentDTO assets);
    public Task Delete(string id);
}

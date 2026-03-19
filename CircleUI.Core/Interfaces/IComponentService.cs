using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IComponentService
{
    public Task<List<Component>> GetAll();
}
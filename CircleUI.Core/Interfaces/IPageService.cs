using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IPageService
{
    public Task<List<Page>> GetAll();
}
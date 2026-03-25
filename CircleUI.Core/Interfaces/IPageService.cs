using CircleUI.Core.DTOs;
using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IPageService
{
    public Task<List<Page>> GetAll();
    public Task<Page> Create(PageDTO pages);
    public Task<PageDTO> GetById(string id);
    public Task<Page> Update(PageDTO pages);
    public Task Delete(string id);
    
}
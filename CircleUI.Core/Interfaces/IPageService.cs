using CircleUI.Core.DTOs;
using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IPageService
{
    public Task<List<PageDTO>> GetAll();
    public Task<PageDTO> Create(PageDTO pages);
    public Task<PageDTO> GetById(string id);
    public Task<PageDTO> Update(PageDTO pages);
    public Task Delete(string id);

}
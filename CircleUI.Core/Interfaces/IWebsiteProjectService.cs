using CircleUI.Core.DTOs;
using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IWebsiteProjectService
{
    public Task<List<WebsiteProjectDTO>> GetAll();
    public Task<List<WebsiteProjectDTO>> GetByUserId(string userId);
    public Task<WebsiteProjectDTO> Create(WebsiteProjectDTO websiteProject);
    public Task<WebsiteProjectDTO> GetById(string id);
    public Task<WebsiteProjectDTO> Update(WebsiteProjectDTO websiteProject);
    public Task Delete(string id);
}
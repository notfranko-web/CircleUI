using CircleUI.Core.DTOs;
using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IWebsiteProjectService
{
    public Task<List<WebsiteProject>> GetAll();
    public Task<WebsiteProject> Create(WebsiteProjectDTO websiteProject);
    public Task<WebsiteProjectDTO> GetById(string id);
    public Task<WebsiteProject> Update(WebsiteProjectDTO websiteProject);
    public Task Delete(string id);
}
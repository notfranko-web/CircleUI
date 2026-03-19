using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IWebsiteProjectService
{
    public Task<List<WebsiteProject>> GetAll();
}
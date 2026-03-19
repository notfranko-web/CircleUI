using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IPublishedVersionService
{
    public Task<List<PublishedVersion>> GetAll();
}
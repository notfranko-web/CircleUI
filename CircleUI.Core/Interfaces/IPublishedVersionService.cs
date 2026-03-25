using CircleUI.Core.DTOs;
using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IPublishedVersionService
{
    public Task<List<PublishedVersion>> GetAll();
    public Task<PublishedVersion> Create(PublishedVersionDTO publishedVersion);
    public Task<PublishedVersionDTO> GetById(string id);
    public Task<PublishedVersion> Update(PublishedVersionDTO publishedVersion);
    public Task Delete(string id);
}
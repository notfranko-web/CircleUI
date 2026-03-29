using CircleUI.Core.DTOs;
using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IPublishedVersionService
{
    public Task<List<PublishedVersionDTO>> GetAll();
    public Task<PublishedVersionDTO> Create(PublishedVersionDTO publishedVersion);
    public Task<PublishedVersionDTO> GetById(string id);
    public Task<PublishedVersionDTO> Update(PublishedVersionDTO publishedVersion);
    public Task Delete(string id);
}
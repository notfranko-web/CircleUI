using CircleUI.Core.DTOs;
using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IUserService
{
    public Task<List<UserDTO>> GetAll();
    public Task<UserDTO> Create(UserDTO user);
    public Task<UserDTO> GetById(string id);
    public Task<UserDTO> Update(UserDTO user);
    public Task Delete(string id);
}
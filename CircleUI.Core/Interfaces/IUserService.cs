using CircleUI.Core.DTOs;
using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IUserService
{
    public Task<List<User>> GetAll();
    public Task<User> Create(UserDTO user);
    public Task<UserDTO> GetById(string id);
    public Task<User> Update(UserDTO user);
    public Task Delete(string id);
}
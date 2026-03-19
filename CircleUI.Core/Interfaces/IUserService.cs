using CircleUI.Data.Models;

namespace CircleUI.Core.Interfaces;

public interface IUserService
{
    public Task<List<User>> GetAll();
}
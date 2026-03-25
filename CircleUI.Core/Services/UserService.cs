using CircleUI.Core.DTOs;
using CircleUI.Core.Interfaces;
using CircleUI.Data;
using CircleUI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CircleUI.Core.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    
    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<User>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> Create(UserDTO input)
    {
        var user = new User()
        {
            DisplayName = input.DisplayName,
            UserName = input.UserName,
            Email = input.Email
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    public async Task<UserDTO> GetById(string id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        
        return new UserDTO()
        {
            DisplayName = user.DisplayName,
            UserName = user.UserName,
            Email = user.Email
        }; 
    }
    
    public async Task<User> Update(UserDTO input)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == input.Id);
        user.DisplayName = input.DisplayName;
        user.UserName = input.UserName;
        user.Email = input.Email;
        await _context.SaveChangesAsync();
        return user;
    }
    
    public async Task Delete(string id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
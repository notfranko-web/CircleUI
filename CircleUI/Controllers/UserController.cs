using CircleUI.Core.DTOs;
using CircleUI.Core.Interfaces;
using CircleUI.Core.Services;
using CircleUI.Data;
using CircleUI.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace CircleUI.Controllers;

public class UserController : Controller
{
    private readonly IUserService _service;

    public UserController(ApplicationDbContext context)
    {
        _service = new UserService(context);
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var users = await _service.GetAll();
        return View(users);
    }
    
    // CREATE
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var user = new UserCreateViewModel();
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateViewModel input)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(input);
        }
        UserDTO user = new UserDTO()
        {
            DisplayName = input.DisplayName,
            UserName = input.UserName,
            Email = input.Email
        }; 
        await _service.Create(user);
        return RedirectToAction("Index");
    }
    
    // UPDATE
    public async Task<IActionResult> Update(string id)
    {
        UserDTO existing = await _service.GetById(id);
        UserUpdateViewModel model = new UserUpdateViewModel()
        {
            DisplayName = existing.DisplayName,
            UserName = existing.UserName,
            Email = existing.Email
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UserUpdateViewModel input)
    {
        UserDTO user = new UserDTO()
        {
            Id = input.Id,
            DisplayName = input.DisplayName,
            UserName = input.UserName,
            Email = input.Email
        };
        ; 
        await _service.Update(user);
        return RedirectToAction("Index");
    }
    
    // DELETE
    public async Task<IActionResult> Delete(string id)
    {
        await _service.Delete(id);
        return RedirectToAction("Index");
    }
}
using CircleUI.Core.Interfaces;
using CircleUI.Core.Services;
using CircleUI.Data;
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
    public IActionResult Index()
    {
        var users = _service.GetAll();
        return View(users);
    }
}
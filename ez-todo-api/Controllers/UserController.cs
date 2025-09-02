using Microsoft.AspNetCore.Mvc;
using EzTodo.Shared.Contracts;
using EzTodo.Shared.DTOs;
using EzTodo.Shared.Models;
using Microsoft.AspNetCore.Identity.Data;
using LoginRequest = EzTodo.Shared.DTOs.LoginRequest;

namespace EzTodo.Api.Controllers;

[ApiController]
[Route("api/user/[controller]")]
public class UserController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var user = authService.ValidateUser(request.Email, request.Password);
        return Ok(user);
    }
    
    [HttpPost("signup")]
    public IActionResult Signup(RegisterRequest request)
    {
        var newUser = new User
        {
            Email = request.Email.Trim().ToLower(),
            Role = Role.User
        };
        
        var user = authService.CreateUser(newUser, request.Password);
        return Ok(user);
    }
    
    [HttpPost("validate")]
    public IActionResult Validate(ValidationRequest request)
    {
        throw new NotImplementedException();
    }
}

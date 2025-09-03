using Microsoft.AspNetCore.Mvc;
using EzTodo.Shared.DTOs;
using EzTodo.Shared.Models;
using EzTodo.Auth;
using System.ComponentModel.DataAnnotations;

namespace EzTodo.Auth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("signup")]
    public ActionResult Signup(SignupRequest request)
    {
        try
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
            {
                return BadRequest(new { error = "Valid email address is required" });
            }

            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
            {
                return BadRequest(new { error = "Password must be at least 6 characters long" });
            }
            
            var user = new User
            {
                Email = request.Email.Trim().ToLower(),
                Role = Role.User
            };

            var success = _authService.CreateUser(user, request.Password);

            if (!success) return BadRequest(new { error = "Failed to create user" });
            
            // Get the created user
            var createdUser = _authService.GetUserByEmail(user.Email);
            if (createdUser == null) return BadRequest(new { error = "Failed to create user" });
            
            var token = _authService.GenerateJwtToken(createdUser);
            return Ok(new
            {
                message = "User registered successfully",
                user = new
                {
                    id = createdUser.Id,
                    email = createdUser.Email,
                    role = createdUser.Role,
                    createdAt = createdUser.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                },
                token = token
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"Registration failed: {ex.Message}" });
        }
    }

    [HttpPost("login")]
    public ActionResult Login(LoginRequest request)
    {
        try
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
            {
                return BadRequest(new { error = "Valid email address is required" });
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { error = "Password is required" });
            }

            var user = _authService.ValidateUser(request.Email.Trim().ToLower(), request.Password);
            
            if (user == null)
            {
                return Unauthorized();
            }

            // Generate JWT token
            var token = _authService.GenerateJwtToken(user);
            
            return Ok(new
            {
                message = "Login successful",
                user = new
                {
                    id = user.Id,
                    name = user.Name,
                    email = user.Email,
                    role = user.Role,
                    createdAt = user.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                },
                token = token
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"Login failed: {ex.Message}" });
        }
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }
        catch
        {
            return false;
        }
    }
}

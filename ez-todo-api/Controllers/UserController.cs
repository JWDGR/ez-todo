using Microsoft.AspNetCore.Mvc;
using EzTodo.Shared.DTOs;
using EzTodo.Api.Services;

namespace EzTodo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserServiceClient userServiceClient) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<ActionResult> Signup(SignupRequest request)
    {
        try
        {
            var response = await userServiceClient.SignupAsync(request);
            return Ok(response);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("400"))
        {
            return BadRequest(new { error = "Invalid request data" });
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("409"))
        {
            return Conflict(new { error = "User already exists" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error during signup: {ex.Message}");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        try
        {
            var response = await userServiceClient.LoginAsync(request);
            return Ok(response);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("400"))
        {
            return BadRequest(new { error = "Invalid request data" });
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("401"))
        {
            return Unauthorized(new { error = "Invalid credentials" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error during login: {ex.Message}");
        }
    }

    [HttpPost("validate")]
    public IActionResult Validate(ValidationRequest request)
    {
        throw new NotImplementedException();
    }
}

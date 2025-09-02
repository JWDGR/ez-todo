namespace EzTodo.Shared.DTOs;

public record LoginRequest(
    string Email, 
    string Password
    );
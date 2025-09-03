namespace EzTodo.Shared.DTOs;

public record UpdateTodoRequest (
    string? Title,
    string? Description,
    bool? IsCompleted
    );
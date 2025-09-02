namespace EzTodo.Shared.DTOs;

public record CreateTodoRequest (
    string Title,
    string? Description
    );
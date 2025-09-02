using EzTodo.Shared.Models;

namespace EzTodo.Shared.DTOs;

public class ValidationResponse
{
    public bool Valid { get; set; }
    public User? User { get; set; }
}
namespace EzTodo.Shared.Models;

public enum Role { User, Admin }

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.User;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
